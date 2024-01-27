using MediatR;
using MongoDB.Driver;
using OmniePDV.Core.Abstractions.Infra;
using OmniePDV.Core.Exceptions;

namespace OmniePDV.Core.CQRS.Manufacturers.Commands.DeleteManufacturer;

internal sealed class DeleteManufacturerCommandHandler(
    IMongoContext mongoContext) : IRequestHandler<DeleteManufacturerCommand>
{
    public readonly IMongoContext _mongoContext = mongoContext;

    public async Task Handle(DeleteManufacturerCommand request, CancellationToken cancellationToken)
    {
        if (request.ID.Equals(Guid.Empty))
            throw new BadRequestException(string.Format("Invalid id {0}", request.ID));

        if (!await _mongoContext.Manufacturers.Find(m => m.UID == request.ID).AnyAsync())
            throw new BadRequestException(string.Format("Manufacturer not found with the id {0}", request.ID));
            
        if (await _mongoContext.Products.Find(m => m.Manufacturer.UID.Equals(request.ID)).AnyAsync())
            throw new BadRequestException("This manufacturer cannot be deleted, because it has products");

        await _mongoContext.Manufacturers.DeleteOneAsync(m => m.UID == request.ID);
    }
}
