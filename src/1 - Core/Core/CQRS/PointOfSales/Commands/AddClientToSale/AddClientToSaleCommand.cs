using MediatR;
using OmniePDV.Core.Entities;

namespace OmniePDV.Core.CQRS.PointOfSales.Commands.AddClientToSale;

public sealed record AddClientToSaleCommand(Guid ClientID) : IRequest<Sale>;
