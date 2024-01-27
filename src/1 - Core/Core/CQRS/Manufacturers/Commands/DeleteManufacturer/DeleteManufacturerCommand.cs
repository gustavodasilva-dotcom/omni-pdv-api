using MediatR;

namespace OmniePDV.Core.CQRS.Manufacturers.Commands.DeleteManufacturer;

public sealed record DeleteManufacturerCommand(Guid ID) : IRequest;
