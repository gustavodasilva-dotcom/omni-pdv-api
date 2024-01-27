using MediatR;
using MongoDB.Driver;
using OmniePDV.Core.Abstractions.Infra;
using OmniePDV.Core.Entities;

namespace OmniePDV.Core.CQRS.Manufacturers.Queries.GetAllManufacturers;

internal sealed class GetManufacturersQuery(IMongoContext mongoContext)
    : IRequestHandler<GetAllManufacturersQuery, List<Manufacturer>>
{
    private readonly IMongoContext _mongoContext = mongoContext;

    public async Task<List<Manufacturer>> Handle(GetAllManufacturersQuery request, CancellationToken cancellationToken)
    {
        return await _mongoContext.Manufacturers.Find(p => true).ToListAsync();
    }
}