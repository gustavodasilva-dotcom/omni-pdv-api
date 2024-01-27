using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OmniePDV.Core.Abstractions.Infra;
using OmniePDV.Core.Configurations;
using OmniePDV.Core.Entities;
using OmniePDV.Core.Enumerations;
using OmniePDV.Core.Exceptions;

namespace OmniePDV.Core.CQRS.PointOfSales.Commands.AddProductToSale;

internal sealed class AddProductToSaleCommandHandler(
    IMongoContext mongoContext,
    IOptions<DefaultClientSettings> defaultClientSettings) : IRequestHandler<AddProductToSaleCommand, Sale>
{
    private readonly IMongoContext _mongoContext = mongoContext;
    private readonly DefaultClientSettings _defaultClientSettings = defaultClientSettings.Value;

    public async Task<Sale> Handle(AddProductToSaleCommand request, CancellationToken cancellationToken)
    {
        Product product = await _mongoContext.Products
            .Find(p => p.Barcode == request.Barcode)
            .FirstOrDefaultAsync() ??
            throw new BadRequestException("Product not found");

        Sale sale = await _mongoContext.Sales
            .Find(s => s.Status == SaleStatusEnum.Open)
            .FirstOrDefaultAsync();

        if (sale is null)
        {
            Client defaultClient = await _mongoContext.Clients
                .Find(c => c.Name.Equals(_defaultClientSettings.Name))
                .FirstOrDefaultAsync();

            long totalSales = _mongoContext.Sales.CountDocuments(s => true);
            sale = new Sale(
                Number: totalSales + 1,
                Subtotal: 0,
                Total: 0,
                Client: defaultClient,
                Products: []
            );
            await _mongoContext.Sales.InsertOneAsync(sale);
        }

        SaleProduct saleProduct = new(
            UID: Guid.NewGuid(),
            Order: sale.Products.Count + 1,
            Quantity: request.Quantity,
            Product: product
        );
        sale.AddProduct(saleProduct);
        
        await _mongoContext.Sales.ReplaceOneAsync(s => s.UID == sale.UID, sale);

        return sale;
    }
}