using MediatR;
using MongoDB.Driver;
using OmniePDV.Core.Abstractions.Infra;
using OmniePDV.Core.Entities;
using OmniePDV.Core.Enumerations;
using OmniePDV.Core.Exceptions;

namespace OmniePDV.Core.CQRS.PointOfSales.Commands.ChangeOpenedSaleStatus;

internal sealed class ChangeOpenedSaleStatusCommandHandler(
    IMongoContext mongoContext) : IRequestHandler<ChangeOpenedSaleStatusCommand, Sale>
{
    private readonly IMongoContext _mongoContext = mongoContext;

    public async Task<Sale> Handle(ChangeOpenedSaleStatusCommand request, CancellationToken cancellationToken)
    {
        Sale sale = await _mongoContext.Sales
            .Find(s => s.Status == SaleStatusEnum.Open)
            .FirstOrDefaultAsync() ??
            throw new BadRequestException("There's no opened sale");

        if (request.Status == SaleStatusEnum.Cancelled)
            sale.CancelSale();
        if (request.Status == SaleStatusEnum.Closed)
            sale.CloseSale();

        await _mongoContext.Sales.ReplaceOneAsync(s => s.UID == sale.UID, sale);

        return sale;
    }
}