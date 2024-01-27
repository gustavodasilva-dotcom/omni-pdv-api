using MediatR;
using MongoDB.Driver;
using OmniePDV.Core.Abstractions.Infra;
using OmniePDV.Core.Entities;
using OmniePDV.Core.Enumerations;
using OmniePDV.Core.Exceptions;

namespace OmniePDV.Core.CQRS.PointOfSales.Commands.DeleteProductFromSale;

internal sealed class DeleteProductFromSaleCommandHandler(
    IMongoContext mongoContext) : IRequestHandler<DeleteProductFromSaleCommand, Sale>
{
    private readonly IMongoContext _mongoContext = mongoContext;

    public async Task<Sale> Handle(DeleteProductFromSaleCommand request, CancellationToken cancellationToken)
    {
        Sale sale = await _mongoContext.Sales
            .Find(s => s.Status == SaleStatusEnum.Open)
            .FirstOrDefaultAsync() ??
            throw new BadRequestException("There's no opened sale");

        SaleProduct productToDelete = sale.Products
            .FirstOrDefault(p => p.Order == request.Order) ??
            throw new BadRequestException(string.Format("There's no product with the order {0}", request.Order));

        productToDelete.DeleteProduct();
        sale.UpdateSubtotal();

        await _mongoContext.Sales.ReplaceOneAsync(s => s.UID == sale.UID, sale);

        return sale;
    }
}
