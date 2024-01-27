using MediatR;
using OmniePDV.Core.Entities;
using OmniePDV.Core.Enumerations;

namespace OmniePDV.Core.CQRS.PointOfSales.Commands.AddDiscountToSale;

public sealed record AddDiscountToSaleCommand(DiscountTypeEnum Type, double Value) : IRequest<Sale>;
