using MediatR;
using OmniePDV.Core.Entities;

namespace OmniePDV.Core.CQRS.Manufacturers.Commands.UpdateManufacturer;

public sealed record UpdateManufacturerCommand(
    Guid ID,
    string Name,
    string CRN,
    bool Active
) : IRequest<Manufacturer>;
