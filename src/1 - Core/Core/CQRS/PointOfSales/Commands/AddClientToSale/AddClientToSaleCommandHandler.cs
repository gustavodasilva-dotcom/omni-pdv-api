using MediatR;
using MongoDB.Driver;
using OmniePDV.Core.Abstractions.Infra;
using OmniePDV.Core.Entities;
using OmniePDV.Core.Enumerations;
using OmniePDV.Core.Exceptions;

namespace OmniePDV.Core.CQRS.PointOfSales.Commands.AddClientToSale;

internal sealed class AddClientToSaleCommandHandler(
    IMongoContext mongoContext) : IRequestHandler<AddClientToSaleCommand, Sale>
{
    private readonly IMongoContext _mongoContext = mongoContext;

    public async Task<Sale> Handle(AddClientToSaleCommand request, CancellationToken cancellationToken)
    {
        Sale sale = await _mongoContext.Sales
            .Find(s => s.Status == SaleStatusEnum.Open)
            .FirstOrDefaultAsync() ??
            throw new BadRequestException("There's no opened sale");

        Client client = await _mongoContext.Clients
            .Find(p => p.UID.Equals(request.ClientID))
            .FirstOrDefaultAsync() ??
            throw new BadRequestException("Client not found");

        sale.SetClient(client);
        await _mongoContext.Sales.ReplaceOneAsync(s =>s.UID == sale.UID, sale);

        return sale;
    }
}
