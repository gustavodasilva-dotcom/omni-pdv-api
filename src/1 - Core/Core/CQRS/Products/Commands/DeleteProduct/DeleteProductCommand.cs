using MediatR;

namespace OmniePDV.Core.CQRS.Products.Commands.DeleteProduct;

public sealed record DeleteProductCommand(Guid ID) : IRequest;
