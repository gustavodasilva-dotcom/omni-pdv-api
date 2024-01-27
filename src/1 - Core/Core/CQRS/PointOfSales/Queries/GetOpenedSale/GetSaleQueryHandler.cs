using MediatR;
using MongoDB.Driver;
using OmniePDV.Core.Abstractions.Infra;
using OmniePDV.Core.Entities;
using OmniePDV.Core.Enumerations;

namespace OmniePDV.Core.CQRS.PointOfSales.Queries.GetOpenedSale;

internal sealed class GetSaleQueryHandler(
    IMongoContext mongoContext) : IRequestHandler<GetOpenedSaleQuery, Sale>
{
    private readonly IMongoContext _mongoContext = mongoContext;

    public async Task<Sale> Handle(GetOpenedSaleQuery request, CancellationToken cancellationToken)
    {
        return await _mongoContext.Sales
            .Find(s => s.Status == SaleStatusEnum.Open)
            .FirstOrDefaultAsync();
    }
}