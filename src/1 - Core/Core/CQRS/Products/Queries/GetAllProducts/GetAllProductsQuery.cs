using MediatR;
using OmniePDV.Core.Entities;

namespace OmniePDV.Core.CQRS.Products.Queries.GetAllProducts;

public sealed record GetAllProductsQuery : IRequest<List<Product>>;
