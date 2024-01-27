using MediatR;
using OmniePDV.Core.Entities;

namespace OmniePDV.Core.CQRS.Products.Commands.UpdateProduct;

public sealed record UpdateProductCommand(
    Guid ID,
    string Name,
    string Description,
    double WholesalePrice,
    double RetailPrice,
    string Barcode,
    Guid ManufacturerID,
    bool Active
) : IRequest<Product>;
