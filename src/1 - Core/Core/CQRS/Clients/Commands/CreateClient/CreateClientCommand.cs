using MediatR;
using OmniePDV.Core.Entities;

namespace OmniePDV.Core.CQRS.Clients.Commands.CreateClient;

public sealed record CreateClientCommand(
    string Name,
    string SSN,
    DateTime Birthday,
    string? Email,
    bool Active
) : IRequest<Client>;