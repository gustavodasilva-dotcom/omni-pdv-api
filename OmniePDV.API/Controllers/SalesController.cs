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
                .Find(s => s.Status == SaleStatus.Open)
                .FirstOrDefaultAsync();
			if (sale == null)
				return NotFound("There's no open sale");

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
                .Find(s => s.Status == SaleStatus.Open)
                .FirstOrDefaultAsync();
			if (sale == null)
			{
                sale = new Data.Entities.Sale(
                    UID: Guid.NewGuid(),
                    Discount: 0,
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
                Quantity: body.Quantity,
                Product: product
            );
            sale.AddProduct(saleProduct);
            sale.UpdateSubtotal(saleProduct.Product.RetailPrice * saleProduct.Quantity);
            sale.UpdateTotal();
            await _context.Sales.ReplaceOneAsync(s => s.UID == sale.UID, sale);

			return Ok(sale.ToViewModel());
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}
