using MediatR;
using MongoDB.Driver;
using OmniePDV.Core.Abstractions.Infra;
using OmniePDV.Core.Entities;
using OmniePDV.Core.Exceptions;

namespace OmniePDV.Core.CQRS.Products.Commands.UpdateProduct;

internal sealed class UpdateProductCommandHandler(
    IMongoContext mongoContext) : IRequestHandler<UpdateProductCommand, Product>
{
    private readonly IMongoContext _mongoContext = mongoContext;

    public async Task<Product> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        Product product = await _mongoContext.Products
            .Find(p => p.UID.Equals(request.ID))
            .FirstOrDefaultAsync() ??
            throw new NotFoundException(string.Format("No product was found with the id {0}", request.ID));

        Manufacturer manufacturer = await _mongoContext.Manufacturers
            .Find(m => m.UID.Equals(request.ManufacturerID))
            .FirstOrDefaultAsync() ??
            throw new BadRequestException("Manufacturer not found");

        product.SetName(request.Name);
        product.SetDescription(request.Description);
        product.SetWholesalePrice(request.WholesalePrice);
        product.SetRetailPrice(request.RetailPrice);
        product.SetBarcode(request.Barcode);
        product.SetManufacturer(manufacturer);
        product.SetActive(request.Active);

        await _mongoContext.Products.ReplaceOneAsync(p => p.UID.Equals(request.ID), product);

        return product;
    }
}
