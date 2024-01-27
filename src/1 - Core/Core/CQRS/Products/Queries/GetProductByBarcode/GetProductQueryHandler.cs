using MediatR;
using MongoDB.Driver;
using OmniePDV.Core.Abstractions.Infra;
using OmniePDV.Core.Entities;
using OmniePDV.Core.Exceptions;

namespace OmniePDV.Core.CQRS.Products.Queries.GetProductByBarcode;

internal sealed class GetProductQueryHandler(
    IMongoContext mongoContext) : IRequestHandler<GetProductByBarcodeQuery, Product>
{
    private readonly IMongoContext _mongoContext = mongoContext;

    public async Task<Product> Handle(GetProductByBarcodeQuery request, CancellationToken cancellationToken)
    {
        return await _mongoContext.Products
            .Find(p => p.Barcode.Equals(request.Barcode.Trim()))
            .FirstOrDefaultAsync() ??
            throw new NotFoundException(string.Format("No product was found with the barcode {0}", request.Barcode));
    }
}
