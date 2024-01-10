using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using OmniePDV.API.Data;
using OmniePDV.API.Entities;
using OmniePDV.API.Models.InputModels;
using OmniePDV.API.Models.ViewModels;

namespace OmniePDV.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SalesController(IMongoContext context) : ControllerBase
{
    private readonly IMongoContext _context = context;

    [HttpGet("get-open-sale")]
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
                sale = new Data.Entities.Sale(
                    UID: Guid.NewGuid(),
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
                return BadRequest("There's no sale opened");

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
                return BadRequest("There's no sale opened");

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
