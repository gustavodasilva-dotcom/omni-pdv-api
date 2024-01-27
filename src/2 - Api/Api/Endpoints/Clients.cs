using System.Net;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OmniePDV.Core.CQRS.Clients.Commands.CreateClient;
using OmniePDV.Core.CQRS.Clients.Commands.DeleteClient;
using OmniePDV.Core.CQRS.Clients.Commands.UpdateClient;
using OmniePDV.Core.CQRS.Clients.Queries.GetAllClients;
using OmniePDV.Core.Entities;
using OmniePDV.Core.Models.InputModels;
using OmniePDV.Core.Models.ViewModels;

namespace OmniePDV.Api.Endpoints;

public class Clients : ICarterModule
{
    const string ROUTE_PREFIX = "/api/v1";
    
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(ROUTE_PREFIX + "/clients/get-all", async (ISender sender) =>
        {
            List<Client> clients = await sender.Send(new GetAllClientsQuery());

            if (clients.Count == 0)
                return Results.NoContent();

            return Results.Ok(new ResultViewModel<List<ClientViewModel>>
            {
                StatusCode = HttpStatusCode.OK,
                Data = clients.ToViewModel()
            });
        });

        app.MapPost(ROUTE_PREFIX + "/clients/add", async ([FromBody] ClientInputModel body, ISender sender) =>
        {
            Client client = await sender.Send(new CreateClientCommand(
                Name: body.Name,
                SSN: body.SSN,
                Birthday: body.Birthday,
                Email: body.Email,
                Active: body.Active
            ));

            return Results.Created(string.Empty, new ResultViewModel<ClientViewModel>
            {
                StatusCode = HttpStatusCode.Created,
                Data = client.ToViewModel()
            });
        });
        
        app.MapPut(ROUTE_PREFIX + "/clients/update/{id:guid}", async (Guid id, [FromBody] ClientInputModel body, ISender sender) =>
        {
            Client client = await sender.Send(new UpdateClientCommand(
                ID: id,
                Name: body.Name,
                SSN: body.SSN,
                Birthday: body.Birthday,
                Email: body.Email,
                Active: body.Active
            ));

            return Results.Ok(new ResultViewModel<ClientViewModel>
            {
                StatusCode = HttpStatusCode.OK,
                Data = client.ToViewModel()
            });
        });
        
        app.MapDelete(ROUTE_PREFIX + "/clients/delete/{id:guid}", async (Guid id, ISender sender) =>
        {
            await sender.Send(new DeleteClientCommand(ID: id));

            return Results.NoContent();
        });
    }
}
