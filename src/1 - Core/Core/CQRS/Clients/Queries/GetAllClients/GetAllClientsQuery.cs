using MediatR;
using OmniePDV.Core.Entities;

namespace OmniePDV.Core.CQRS.Clients.Queries.GetAllClients;

public sealed record GetAllClientsQuery : IRequest<List<Client>>;
