using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using OmniePDV.API.Data;
using OmniePDV.API.Data.Entities;
using OmniePDV.API.Models.InputModels;
using OmniePDV.API.Models.ViewModels;

namespace OmniePDV.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ManufacturersController(IMongoContext context) : ControllerBase
{
    private readonly IMongoContext _context = context;

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllManufacturers()
    {
        try
        {
            List<Manufacturer> manufacturers = await _context.Manufacturers.Find(p => true).ToListAsync();
            if (manufacturers.Count == 0)
                return NoContent();

            return Ok(manufacturers.ToViewModel());
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddManufacturer([FromBody] ManufacturerInputModel body)
    {
        try
        {
            Manufacturer manufacturer = await _context.Manufacturers
                .Find(m => m.CRN.Equals(body.CRN.Trim()))
                .FirstOrDefaultAsync();
            if (manufacturer != null)
                return Conflict(string.Format("There's already a manufacturer with the CRN {0}", body.CRN));

            manufacturer = new Manufacturer(
                UID: Guid.NewGuid(),
                Name: body.Name.Trim(),
                CRN: body.CRN.Trim(),
                Active: body.Active
            );
            await _context.Manufacturers.InsertOneAsync(manufacturer);

            return Created(string.Empty, manufacturer.ToViewModel());
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}
