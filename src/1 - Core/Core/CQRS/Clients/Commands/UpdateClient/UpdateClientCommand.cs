using MediatR;
using OmniePDV.Core.Entities;

namespace OmniePDV.Core.CQRS.Clients.Commands.UpdateClient;

public sealed record UpdateClientCommand(
    Guid ID,
    string Name,
    string SSN,
    DateTime Birthday,
    string? Email,
    bool Active
) : IRequest<Client>;
