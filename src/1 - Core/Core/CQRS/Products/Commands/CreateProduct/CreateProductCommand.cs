using MediatR;
using OmniePDV.Core.Entities;

namespace OmniePDV.Core.CQRS.Products.Commands.CreateProduct;

public sealed record CreateProductCommand(
    string Name,
    string Description,
    double WholesalePrice,
    double RetailPrice,
    string Barcode,
    Guid ManufacturerID,
    bool Active
) : IRequest<Product>;
