using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using OmniePDV.API.Data;
using OmniePDV.API.Data.Entities;
using OmniePDV.API.Models.InputModels;
using OmniePDV.API.Models.ViewModels;

namespace OmniePDV.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(IMongoContext context) : ControllerBase
{
    private readonly IMongoContext _context = context;

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllProducts()
    {
        try
        {
            List<Product> products = await _context.Products.Find(p => true).ToListAsync();
            if (products.Count == 0)
                return NoContent();

            return Ok(products.ToViewModel());
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpGet("get-by-barcode/{barcode}")]
    public async Task<IActionResult> GetProductByBarcode([FromRoute] string barcode)
    {
        try
        {
            Product product = await _context.Products
                .Find(p => p.Barcode.Equals(barcode.Trim()))
                .FirstOrDefaultAsync();
            if (product == null)
                return NotFound(string.Format("No product was found with the barcode {0}", barcode));

            return Ok(product.ToViewModel());
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddProduct([FromBody] ProductInputModel body)
    {
        try
        {
            Manufacturer manufacturer = await _context.Manufacturers
                .Find(m => m.UID == body.ManufacturerID)
                .FirstOrDefaultAsync();
            if (manufacturer == null)
                return BadRequest("Manufacturer not found");

            Product product = await _context.Products
                .Find(p => p.Barcode.Equals(body.Barcode.Trim()))
                .FirstOrDefaultAsync();
            if (product != null)
                return Conflict(string.Format("There's already a product with the Barcode {0}", body.Barcode.Trim()));

            product = new(
                UID: Guid.NewGuid(),
                Name: body.Name.Trim(),
                Description: body.Description.Trim(),
                WholesalePrice: body.WholesalePrice,
                RetailPrice: body.RetailPrice,
                Barcode: body.Barcode.Trim(),
                Manufacturer: manufacturer,
                Active: body.Active
            );
            await _context.Products.InsertOneAsync(product);

            return Created(string.Empty, product.ToViewModel());
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}
