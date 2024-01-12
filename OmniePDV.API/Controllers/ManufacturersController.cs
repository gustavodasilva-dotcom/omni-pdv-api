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
public class ManufacturersController(IMongoContext context) : ControllerBase
{
    private readonly IMongoContext _context = context;

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllManufacturers()
    {
        List<Manufacturer> manufacturers = await _context.Manufacturers
            .Find(p => true).ToListAsync();
        if (manufacturers.Count == 0)
            return NoContent();

        return Ok(JsonResultViewModel<List<ManufacturerViewModel>>
            .New(HttpStatusCode.OK, manufacturers.ToViewModel()));
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddManufacturer([FromBody] ManufacturerInputModel body)
    {
        Manufacturer manufacturer = await _context.Manufacturers
            .Find(m => m.CRN.Equals(body.CRN.Trim()))
            .FirstOrDefaultAsync();
        if (manufacturer != null)
            throw new ConflictException(string.Format("There's already a manufacturer with the CRN {0}", body.CRN));

        manufacturer = new Manufacturer(
            Name: body.Name,
            CRN: body.CRN,
            Active: body.Active
        );
        await _context.Manufacturers.InsertOneAsync(manufacturer);

        return Created(string.Empty, JsonResultViewModel<ManufacturerViewModel>
            .New(HttpStatusCode.Created, manufacturer.ToViewModel()));
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateManufacturer([FromRoute] Guid id, [FromBody] ManufacturerInputModel body)
    {
        Manufacturer manufacturer = await _context.Manufacturers
            .Find(m => m.UID.Equals(id))
            .FirstOrDefaultAsync() ??
            throw new NotFoundException(string.Format("No manufacturer was found with the id {0}", id));

        manufacturer.SetName(body.Name);
        manufacturer.SetCRN(body.CRN);
        manufacturer.SetActive(body.Active);

        await _context.Manufacturers.ReplaceOneAsync(m => m.UID.Equals(id), manufacturer);

        return Ok(JsonResultViewModel<ManufacturerViewModel>
            .New(HttpStatusCode.OK, manufacturer.ToViewModel()));
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteManufacturer([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
            throw new BadRequestException(string.Format("Invalid id {0}", id));
        if (!await _context.Manufacturers.Find(m => m.UID == id).AnyAsync())
            throw new BadRequestException(string.Format("Manufacturer not found with the id {0}", id));
        if (await _context.Products.Find(m => m.Manufacturer.UID.Equals(id)).AnyAsync())
            throw new BadRequestException("This manufacturer cannot be deleted, because it has products");

        await _context.Manufacturers.DeleteOneAsync(m => m.UID == id);

        return NoContent();
    }
}
