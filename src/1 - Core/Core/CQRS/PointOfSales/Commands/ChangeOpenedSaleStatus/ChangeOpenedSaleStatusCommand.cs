using MediatR;
using OmniePDV.Core.Entities;
using OmniePDV.Core.Enumerations;

namespace OmniePDV.Core.CQRS.PointOfSales.Commands.ChangeOpenedSaleStatus;

public sealed record ChangeOpenedSaleStatusCommand(SaleStatusEnum Status) : IRequest<Sale>;
