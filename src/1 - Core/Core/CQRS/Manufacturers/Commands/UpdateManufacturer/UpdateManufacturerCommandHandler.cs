using MediatR;
using MongoDB.Driver;
using OmniePDV.Core.Abstractions.Infra;
using OmniePDV.Core.Entities;
using OmniePDV.Core.Exceptions;

namespace OmniePDV.Core.CQRS.Manufacturers.Commands.UpdateManufacturer;

internal sealed class UpdateManufacturerCommandHandler(
    IMongoContext mongoContext) : IRequestHandler<UpdateManufacturerCommand, Manufacturer>
{
    private readonly IMongoContext _mongoContext = mongoContext;

    public async Task<Manufacturer> Handle(UpdateManufacturerCommand request, CancellationToken cancellationToken)
    {
        Manufacturer manufacturer = await _mongoContext.Manufacturers
            .Find(m => m.UID.Equals(request.ID))
            .FirstOrDefaultAsync() ??
            throw new NotFoundException(string.Format("No manufacturer was found with the id {0}", request.ID));

        manufacturer.SetName(request.Name);
        manufacturer.SetCRN(request.CRN);
        manufacturer.SetActive(request.Active);

        await _mongoContext.Manufacturers.ReplaceOneAsync(m => m.UID.Equals(request.ID), manufacturer);

        return manufacturer;
    }
}
