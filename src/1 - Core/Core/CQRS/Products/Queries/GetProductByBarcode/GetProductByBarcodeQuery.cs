using MediatR;
using OmniePDV.Core.Entities;

namespace OmniePDV.Core.CQRS.Products.Queries.GetProductByBarcode;

public sealed record GetProductByBarcodeQuery(string Barcode) : IRequest<Product>;
