using MediatR;
using MongoDB.Driver;
using OmniePDV.Core.Abstractions.Infra;
using OmniePDV.Core.Entities;
using OmniePDV.Core.Exceptions;

namespace OmniePDV.Core.CQRS.Products.Commands.CreateProduct;

internal sealed class CreateProductCommandHandler(
    IMongoContext mongoContext) : IRequestHandler<CreateProductCommand, Product>
{
    private readonly IMongoContext _mongoContext = mongoContext;

    public async Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        Manufacturer manufacturer = await _mongoContext.Manufacturers
            .Find(m => m.UID.Equals(request.ManufacturerID))
            .FirstOrDefaultAsync() ??
            throw new BadRequestException("Manufacturer not found");

        Product product = await _mongoContext.Products
            .Find(p => p.Barcode.Equals(request.Barcode.Trim()))
            .FirstOrDefaultAsync();

        if (product is not null)
            throw new ConflictException(string.Format("There's already a product with the Barcode {0}",
                request.Barcode.Trim()));

        product = new(
            Name: request.Name,
            Description: request.Description,
            WholesalePrice: request.WholesalePrice,
            RetailPrice: request.RetailPrice,
            Barcode: request.Barcode,
            Manufacturer: manufacturer,
            Active: request.Active
        );
        await _mongoContext.Products.InsertOneAsync(product);

        return product;
    }
}
