using MediatR;
using MongoDB.Driver;
using OmniePDV.Core.Abstractions.Infra;
using OmniePDV.Core.Entities;

namespace OmniePDV.Core.CQRS.Clients.Queries.GetAllClients;

internal sealed class GetClientsQueryHandler(
    IMongoContext mongoContext) : IRequestHandler<GetAllClientsQuery, List<Client>>
{
    private readonly IMongoContext _mongoContext = mongoContext;

    public async Task<List<Client>> Handle(GetAllClientsQuery request, CancellationToken cancellationToken)
    {
        return await _mongoContext.Clients.Find(p => true).ToListAsync();
    }
}
