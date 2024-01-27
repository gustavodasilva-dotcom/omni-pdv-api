using MediatR;
using OmniePDV.Core.Entities;

namespace OmniePDV.Core.CQRS.PointOfSales.Commands.SendSaleReceiptToEmail;

public sealed record SendSaleReceiptToEmailCommand(Guid SaleID, string Email) : IRequest;
