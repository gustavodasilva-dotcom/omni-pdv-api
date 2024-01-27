using MediatR;
using OmniePDV.Core.Entities;

namespace OmniePDV.Core.CQRS.PointOfSales.Commands.DeleteProductFromSale;

public sealed record DeleteProductFromSaleCommand(int Order) : IRequest<Sale>;
