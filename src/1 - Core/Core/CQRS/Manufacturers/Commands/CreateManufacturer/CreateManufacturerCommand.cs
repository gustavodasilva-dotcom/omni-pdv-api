using MediatR;
using OmniePDV.Core.Entities;

namespace OmniePDV.Core.CQRS.Manufacturers.Commands.CreateManufacturer;

public sealed record CreateManufacturerCommand(
    string Name,
    string CRN,
    bool Active
) : IRequest<Manufacturer>;
