using MediatR;
using OmniePDV.Core.Entities;

namespace OmniePDV.Core.CQRS.PointOfSales.Commands.AddProductToSale;

public sealed record AddProductToSaleCommand(string Barcode, double Quantity) : IRequest<Sale>;
