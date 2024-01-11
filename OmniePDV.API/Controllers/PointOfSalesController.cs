using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using OmniePDV.API.Data;
using OmniePDV.API.Entities;
using OmniePDV.API.Models.InputModels;
using OmniePDV.API.Models.ViewModels;

namespace OmniePDV.API.Controllers;

[Route("api/point-of-sales")]
[ApiController]
public class PointOfSalesController(IMongoContext context) : ControllerBase
{
    private readonly IMongoContext _context = context;

    [HttpGet("get-opened-sale")]
    public async Task<IActionResult> GetOpenSale()
    {
		try
		{
			Data.Entities.Sale sale = await _context.Sales
                .Find(s => s.Status == SaleStatusEnum.Open)
                .FirstOrDefaultAsync();
			if (sale == null)
				return NoContent();

			return Ok(sale.ToViewModel());
		}
		catch (Exception e)
		{
			return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
		}
    }

    [HttpPut("change-opened-sale-status")]
    public async Task<IActionResult> CloseOpenedSale([FromBody] ChangeOpenedSaleStatusInputModel body)
    {
        try
        {
            Data.Entities.Sale sale = await _context.Sales
                .Find(s => s.Status == SaleStatusEnum.Open)
                .FirstOrDefaultAsync();
            if (sale == null)
                return BadRequest("There's no opened sale");

            if (body.Status == SaleStatusEnum.Closed)
                sale.CloseSale();
            if (body.Status == SaleStatusEnum.Cancelled)
                sale.CancelSale();

            await _context.Sales.ReplaceOneAsync(s => s.UID == sale.UID, sale);

            if (sale.Status == SaleStatusEnum.Open)
                return Ok(sale.ToViewModel());
            else
                return NoContent();
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpPost("add-product")]
	public async Task<IActionResult> AddProductToSale([FromBody] AddProductToSaleInputModel body)
	{
		try
		{
            Data.Entities.Sale sale = await _context.Sales
                .Find(s => s.Status == SaleStatusEnum.Open)
                .FirstOrDefaultAsync();
			if (sale == null)
			{
                long totalSales = _context.Sales.CountDocuments(s => true);
                sale = new Data.Entities.Sale(
                    UID: Guid.NewGuid(),
                    Number: totalSales + 1,
                    Subtotal: 0,
                    Total: 0,
                    Products: []
                );
                await _context.Sales.InsertOneAsync(sale);
            }

            Data.Entities.Product product = await _context.Products
                .Find(p => p.Barcode == body.Barcode)
                .FirstOrDefaultAsync();
            if (product == null)
                return BadRequest("Product not found");

            Data.Entities.SaleProduct saleProduct = new(
                UID: Guid.NewGuid(),
                Order: sale.Products.Count + 1,
                Quantity: body.Quantity,
                Product: product
            );
            sale.AddProduct(saleProduct);
            await _context.Sales.ReplaceOneAsync(s => s.UID == sale.UID, sale);

			return Ok(sale.ToViewModel());
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpDelete("delete-product/{order:int}")]
    public async Task<IActionResult> DeleteProductFromSale([FromRoute] int order)
    {
        try
        {
            Data.Entities.Sale sale = await _context.Sales
                .Find(s => s.Status == SaleStatusEnum.Open)
                .FirstOrDefaultAsync();
			if (sale == null)
                return BadRequest("There's no opened sale");

            Data.Entities.SaleProduct? productToDelete = sale.Products.FirstOrDefault(p => p.Order == order);
            if (productToDelete == null)
                return BadRequest(string.Format("There's no product with the order {0}", order));

            productToDelete.DeleteProduct();
            sale.UpdateSubtotal();
            await _context.Sales.ReplaceOneAsync(s => s.UID == sale.UID, sale);

            return Ok(sale.ToViewModel());
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpPatch("add-discount")]
    public async Task<IActionResult> AddDiscountToSale([FromBody] AddDiscountToSaleInputModel body)
    {
        try
        {
            Data.Entities.Sale sale = await _context.Sales
                .Find(s => s.Status == SaleStatusEnum.Open)
                .FirstOrDefaultAsync();
			if (sale == null)
                return BadRequest("There's no opened sale");

            sale.AddDiscount(body.Value, body.Type);
            await _context.Sales.ReplaceOneAsync(s => s.UID == sale.UID, sale);

            return Ok(sale.ToViewModel());
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}
