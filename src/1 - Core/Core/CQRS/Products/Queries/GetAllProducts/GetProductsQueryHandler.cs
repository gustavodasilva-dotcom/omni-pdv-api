using MediatR;
using MongoDB.Driver;
using OmniePDV.Core.Abstractions.Infra;
using OmniePDV.Core.Entities;

namespace OmniePDV.Core.CQRS.Products.Queries.GetAllProducts;

internal sealed class GetProductsQueryHandler(
    IMongoContext mongoContext) : IRequestHandler<GetAllProductsQuery, List<Product>>
{
    private readonly IMongoContext _mongoContext = mongoContext;

    public async Task<List<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        return await _mongoContext.Products.Find(p => true).ToListAsync();
    }
}
