using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using OmniePDV.API.Data;
using OmniePDV.API.Data.Entities;
using OmniePDV.API.Models.ViewModels.Base;
using OmniePDV.API.Models.ViewModels;
using System.Net;
using OmniePDV.API.Models.InputModels;
using OmniePDV.API.Exceptions;
using Microsoft.Extensions.Options;
using OmniePDV.API.Options;

namespace OmniePDV.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class ClientsController(
    IMongoContext context,
    IOptions<DefaultClientOptions> options) : ControllerBase
{
    private readonly IMongoContext _context = context;
    private readonly DefaultClientOptions _defaultClientOptions = options.Value;

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllClients()
    {
        List<Client> clients = await _context.Clients.Find(p => true).ToListAsync();
        if (clients.Count == 0)
            return NoContent();

        return Ok(JsonResultViewModel<List<ClientViewModel>>
            .New(HttpStatusCode.OK, clients.ToViewModel()));
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddClient([FromBody] ClientInputModel body)
    {
        Client defaultClient = await _context.Clients
            .Find(c => c.Name.Equals(_defaultClientOptions.Name))
            .FirstOrDefaultAsync();
        if (body.Name.Trim().ToLower().Equals(defaultClient.Name.Trim().ToLower()))
            throw new ConflictException(string.Format("The name {0} is reserved", defaultClient.Name));

        Client client = await _context.Clients
            .Find(c => c.SSN.Equals(body.SSN))
            .FirstOrDefaultAsync();
        if (client != null)
            throw new ConflictException(string.Format("There's already a client with the SSN {0}", body.SSN));

        client = new(
            name: body.Name,
            ssn: body.SSN,
            birthday: body.Birthday,
            email: body.Email,
            active: body.Active
        );
        await _context.Clients.InsertOneAsync(client);

        return Created(string.Empty, JsonResultViewModel<ClientViewModel>
            .New(HttpStatusCode.Created, client.ToViewModel()));
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateClient([FromRoute] Guid id, [FromBody] ClientInputModel body)
    {
        Client defaultClient = await _context.Clients
            .Find(c => c.Name.Equals(_defaultClientOptions.Name))
            .FirstOrDefaultAsync();
        if (body.Name.Trim().ToLower().Equals(defaultClient.Name.Trim().ToLower()))
            throw new ConflictException(string.Format("The name {0} is reserved", defaultClient.Name));

        Client client = await _context.Clients
            .Find(m => m.UID.Equals(id))
            .FirstOrDefaultAsync() ??
            throw new NotFoundException(string.Format("No client was found with the id {0}", id));

        client.SetName(body.Name);
        client.SetSSN(body.SSN);
        client.SetBirthday(body.Birthday);
        client.SetEmail(body.Email);
        client.SetActive(body.Active);

        await _context.Clients.ReplaceOneAsync(c => c.UID.Equals(id), client);

        return Ok(JsonResultViewModel<ClientViewModel>
            .New(HttpStatusCode.OK, client.ToViewModel()));
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteClient([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
            throw new BadRequestException(string.Format("Invalid id {0}", id));
        if (!await _context.Clients.Find(m => m.UID == id).AnyAsync())
            throw new BadRequestException(string.Format("Client not found with the id {0}", id));
        if (await _context.Sales.Find(s => s.Client != null && s.Client.UID.Equals(id)).AnyAsync())
            throw new BadRequestException("This client cannot be deleted, because it has sales history");

        await _context.Clients.DeleteOneAsync(m => m.UID == id);

        return NoContent();
    }
}
