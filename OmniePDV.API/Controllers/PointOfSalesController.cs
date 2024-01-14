using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OmniePDV.API.Data;
using OmniePDV.API.Entities;
using OmniePDV.API.Exceptions;
using OmniePDV.API.Models.InputModels;
using OmniePDV.API.Models.ViewModels;
using OmniePDV.API.Models.ViewModels.Base;
using OmniePDV.API.Options.Global;
using System.Net;

namespace OmniePDV.API.Controllers;

[Route("api/point-of-sales")]
[ApiController]
[Produces("application/json")]
public class PointOfSalesController(
    IMongoContext context,
    IOptions<GlobalOptions> globalOptions) : ControllerBase
{
    private readonly IMongoContext _context = context;
    private readonly GlobalOptions _globalOptions = globalOptions.Value;

    [HttpGet("get-opened-sale")]
    public async Task<IActionResult> GetOpenSale()
    {
        Data.Entities.Sale sale = await _context.Sales
            .Find(s => s.Status == SaleStatusEnum.Open)
            .FirstOrDefaultAsync();
        if (sale == null)
            return NoContent();

        return Ok(JsonResultViewModel<POSViewModel>
            .New(HttpStatusCode.OK, sale.ToViewModel()));
    }

    [HttpPost("add-product")]
    public async Task<IActionResult> AddProductToSale([FromBody] AddProductToSaleInputModel body)
    {
        Data.Entities.Product product = await _context.Products
            .Find(p => p.Barcode == body.Barcode)
            .FirstOrDefaultAsync() ??
            throw new BadRequestException("Product not found");
            
        Data.Entities.Sale sale = await _context.Sales
            .Find(s => s.Status == SaleStatusEnum.Open)
            .FirstOrDefaultAsync();
        if (sale == null)
        {
            Data.Entities.Client defaultClient = await _context.Clients
                .Find(c => c.Name.Equals(_globalOptions.Client.Default.Name))
                .FirstOrDefaultAsync();

            long totalSales = _context.Sales.CountDocuments(s => true);
            sale = new Data.Entities.Sale(
                Number: totalSales + 1,
                Subtotal: 0,
                Total: 0,
                Client: defaultClient,
                Products: []
            );
            await _context.Sales.InsertOneAsync(sale);
        }

        Data.Entities.SaleProduct saleProduct = new(
            UID: Guid.NewGuid(),
            Order: sale.Products.Count + 1,
            Quantity: body.Quantity,
            Product: product
        );
        sale.AddProduct(saleProduct);
        await _context.Sales.ReplaceOneAsync(s => s.UID == sale.UID, sale);

        return Ok(JsonResultViewModel<POSViewModel>
            .New(HttpStatusCode.OK, sale.ToViewModel()));
    }

    [HttpPut("change-opened-sale-status")]
    public async Task<IActionResult> CloseOpenedSale([FromBody] ChangeOpenedSaleStatusInputModel body)
    {
        Data.Entities.Sale sale = await _context.Sales
            .Find(s => s.Status == SaleStatusEnum.Open)
            .FirstOrDefaultAsync() ??
            throw new BadRequestException("There's no opened sale");

        if (body.Status == SaleStatusEnum.Closed)
            sale.CloseSale();
        if (body.Status == SaleStatusEnum.Cancelled)
            sale.CancelSale();

        await _context.Sales.ReplaceOneAsync(s => s.UID == sale.UID, sale);

        if (sale.Status == SaleStatusEnum.Open)
            return Ok(JsonResultViewModel<POSViewModel>
                .New(HttpStatusCode.OK, sale.ToViewModel()));
        else
            return NoContent();
    }

    [HttpPatch("add-discount")]
    public async Task<IActionResult> AddDiscountToSale([FromBody] AddDiscountToSaleInputModel body)
    {
        Data.Entities.Sale sale = await _context.Sales
            .Find(s => s.Status == SaleStatusEnum.Open)
            .FirstOrDefaultAsync() ??
            throw new BadRequestException("There's no opened sale");

        sale.AddDiscount(body.Value, body.Type);
        await _context.Sales.ReplaceOneAsync(s => s.UID == sale.UID, sale);

        return Ok(JsonResultViewModel<POSViewModel>
            .New(HttpStatusCode.OK, sale.ToViewModel()));
    }

    [HttpPatch("add-client")]
    public async Task<IActionResult> AddClientToSale([FromBody] AddClientToSaleInputModel body)
    {
        Data.Entities.Sale sale = await _context.Sales
            .Find(s => s.Status == SaleStatusEnum.Open)
            .FirstOrDefaultAsync() ??
            throw new BadRequestException("There's no opened sale");

        Data.Entities.Client client = await _context.Clients
            .Find(p => p.UID.Equals(body.ClientID))
            .FirstOrDefaultAsync() ??
            throw new BadRequestException("Client not found");

        sale.SetClient(client);
        await _context.Sales.ReplaceOneAsync(s => s.UID == sale.UID, sale);

        return Ok(JsonResultViewModel<POSViewModel>
            .New(HttpStatusCode.OK, sale.ToViewModel()));
    }

    [HttpDelete("delete-product/{order:int}")]
    public async Task<IActionResult> DeleteProductFromSale([FromRoute] int order)
    {
        Data.Entities.Sale sale = await _context.Sales
            .Find(s => s.Status == SaleStatusEnum.Open)
            .FirstOrDefaultAsync() ??
            throw new BadRequestException("There's no opened sale");

        Data.Entities.SaleProduct? productToDelete = sale.Products
            .FirstOrDefault(p => p.Order == order) ??
            throw new BadRequestException(string.Format("There's no product with the order {0}", order));

        productToDelete.DeleteProduct();
        sale.UpdateSubtotal();
        await _context.Sales.ReplaceOneAsync(s => s.UID == sale.UID, sale);

        return Ok(JsonResultViewModel<POSViewModel>
            .New(HttpStatusCode.OK, sale.ToViewModel()));
    }
}
