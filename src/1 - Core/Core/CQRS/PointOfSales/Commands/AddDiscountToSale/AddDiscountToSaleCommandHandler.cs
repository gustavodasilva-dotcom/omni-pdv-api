using MediatR;
using MongoDB.Driver;
using OmniePDV.Core.Abstractions.Infra;
using OmniePDV.Core.Entities;
using OmniePDV.Core.Enumerations;
using OmniePDV.Core.Exceptions;

namespace OmniePDV.Core.CQRS.PointOfSales.Commands.AddDiscountToSale;

internal sealed class AddDiscountToSaleCommandHandler(
    IMongoContext mongoContext) : IRequestHandler<AddDiscountToSaleCommand, Sale>
{
    private readonly IMongoContext _mongoContext = mongoContext;

    public async Task<Sale> Handle(AddDiscountToSaleCommand request, CancellationToken cancellationToken)
    {
        Sale sale = await _mongoContext.Sales
            .Find(s => s.Status == SaleStatusEnum.Open)
            .FirstOrDefaultAsync() ??
            throw new BadRequestException("There's no opened sale");

        sale.AddDiscount(request.Value, request.Type);
        await _mongoContext.Sales.ReplaceOneAsync(s => s.UID == sale.UID, sale);

        return sale;
    }
}