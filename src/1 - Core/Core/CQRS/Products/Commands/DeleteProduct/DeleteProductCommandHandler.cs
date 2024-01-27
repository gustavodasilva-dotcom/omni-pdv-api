using MediatR;
using MongoDB.Driver;
using OmniePDV.Core.Abstractions.Infra;
using OmniePDV.Core.Exceptions;

namespace OmniePDV.Core.CQRS.Products.Commands.DeleteProduct;

internal sealed class DeleteProductCommandHandler(
    IMongoContext mongoContext) : IRequestHandler<DeleteProductCommand>
{
    private readonly IMongoContext _mongoContext = mongoContext;

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        if (request.ID.Equals(Guid.Empty))
            throw new BadRequestException(string.Format("Invalid id {0}", request.ID));

        if (!await _mongoContext.Products.Find(p => p.UID.Equals(request.ID)).AnyAsync())
            throw new BadRequestException(string.Format("Product not found with the id {0}", request.ID));
        
        if (await _mongoContext.Sales.Find(s => s.Products.Any(p => p.Product.UID.Equals(request.ID))).AnyAsync())
            throw new BadRequestException("This product cannot be deleted, because it has sales history");

        await _mongoContext.Products.DeleteOneAsync(p => p.UID.Equals(request.ID));
    }
}
