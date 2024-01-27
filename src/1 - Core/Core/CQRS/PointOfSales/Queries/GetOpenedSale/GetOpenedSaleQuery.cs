using MediatR;
using OmniePDV.Core.Entities;

namespace OmniePDV.Core.CQRS.PointOfSales.Queries.GetOpenedSale;

public sealed record GetOpenedSaleQuery : IRequest<Sale>;
