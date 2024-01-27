using MediatR;
using MongoDB.Driver;
using OmniePDV.Core.Abstractions.Infra;
using OmniePDV.Core.Entities;
using OmniePDV.Core.Exceptions;

namespace OmniePDV.Core.CQRS.Manufacturers.Commands.CreateManufacturer;

internal sealed class CreateManufacturerCommandHandler(
    IMongoContext mongoContext) : IRequestHandler<CreateManufacturerCommand, Manufacturer>
{
    private readonly IMongoContext _mongoContext = mongoContext;

    public async Task<Manufacturer> Handle(CreateManufacturerCommand request, CancellationToken cancellationToken)
    {
        Manufacturer manufacturer = await _mongoContext.Manufacturers
            .Find(m => m.CRN.Equals(request.CRN.Trim()))
            .FirstOrDefaultAsync();
        if (manufacturer is not null)
            throw new ConflictException(string.Format("There's already a manufacturer with the CRN {0}", request.CRN));

        manufacturer = new Manufacturer(
            Name: request.Name,
            CRN: request.CRN,
            Active: request.Active
        );
        await _mongoContext.Manufacturers.InsertOneAsync(manufacturer);

        return manufacturer;
    }
}