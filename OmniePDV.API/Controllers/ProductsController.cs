using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using OmniePDV.API.Data;
using OmniePDV.API.Data.Entities;
using OmniePDV.API.Exceptions;
using OmniePDV.API.Models.InputModels;
using OmniePDV.API.Models.ViewModels;
using OmniePDV.API.Models.ViewModels.Base;
using System.Net;

namespace OmniePDV.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class ProductsController(IMongoContext context) : ControllerBase
{
    private readonly IMongoContext _context = context;

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllProducts()
    {
        List<Product> products = await _context.Products.Find(p => true).ToListAsync();
        if (products.Count == 0)
            return NoContent();

        return Ok(JsonResultViewModel<List<ProductViewModel>>
            .New(HttpStatusCode.OK, products.ToViewModel()));
    }

    [HttpGet("get-by-barcode/{barcode}")]
    public async Task<IActionResult> GetProductByBarcode([FromRoute] string barcode)
    {
        Product product = await _context.Products
            .Find(p => p.Barcode.Equals(barcode.Trim()))
            .FirstOrDefaultAsync() ??
            throw new NotFoundException(string.Format("No product was found with the barcode {0}", barcode));

        return Ok(JsonResultViewModel<ProductViewModel>
            .New(HttpStatusCode.OK, product.ToViewModel()));
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddProduct([FromBody] ProductInputModel body)
    {
        Manufacturer manufacturer = await _context.Manufacturers
            .Find(m => m.UID.Equals(body.ManufacturerID))
            .FirstOrDefaultAsync() ??
            throw new BadRequestException("Manufacturer not found");

        Product product = await _context.Products
            .Find(p => p.Barcode.Equals(body.Barcode.Trim()))
            .FirstOrDefaultAsync();

        if (product != null)
            throw new ConflictException(string.Format("There's already a product with the Barcode {0}",
                body.Barcode.Trim()));

        product = new(
            Name: body.Name,
            Description: body.Description,
            WholesalePrice: body.WholesalePrice,
            RetailPrice: body.RetailPrice,
            Barcode: body.Barcode,
            Manufacturer: manufacturer,
            Active: body.Active
        );
        await _context.Products.InsertOneAsync(product);

        return Created(string.Empty, JsonResultViewModel<ProductViewModel>
            .New(HttpStatusCode.Created, product.ToViewModel()));
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateProduct([FromRoute] Guid id, [FromBody] ProductInputModel body)
    {
        Product product = await _context.Products
            .Find(p => p.UID.Equals(id))
            .FirstOrDefaultAsync() ??
            throw new NotFoundException(string.Format("No product was found with the id {0}", id));

        Manufacturer manufacturer = await _context.Manufacturers
            .Find(m => m.UID == body.ManufacturerID)
            .FirstOrDefaultAsync() ??
            throw new BadRequestException("Manufacturer not found");

        product.SetName(body.Name);
        product.SetDescription(body.Description);
        product.SetWholesalePrice(body.WholesalePrice);
        product.SetRetailPrice(body.RetailPrice);
        product.SetBarcode(body.Barcode);
        product.SetManufacturer(manufacturer);
        product.SetActive(body.Active);

        await _context.Products.ReplaceOneAsync(p => p.UID.Equals(id), product);

        return Ok(JsonResultViewModel<ProductViewModel>
            .New(HttpStatusCode.OK, product.ToViewModel()));
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteProduct([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
            throw new BadRequestException(string.Format("Invalid id {0}", id));
        if (!await _context.Products.Find(p => p.UID == id).AnyAsync())
            throw new BadRequestException(string.Format("Product not found with the id {0}", id));
        if (await _context.Sales.Find(s => s.Products.Any(p => p.Product.UID == id)).AnyAsync())
            throw new BadRequestException("This product cannot be deleted, because it has sales history");

        await _context.Products.DeleteOneAsync(p => p.UID.Equals(id));
        return NoContent();
    }
}
