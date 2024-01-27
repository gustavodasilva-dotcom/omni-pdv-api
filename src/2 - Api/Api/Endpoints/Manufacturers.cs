using System.Net;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OmniePDV.Core.CQRS.Manufacturers.Commands.CreateManufacturer;
using OmniePDV.Core.CQRS.Manufacturers.Commands.DeleteManufacturer;
using OmniePDV.Core.CQRS.Manufacturers.Commands.UpdateManufacturer;
using OmniePDV.Core.CQRS.Manufacturers.Queries.GetAllManufacturers;
using OmniePDV.Core.Entities;
using OmniePDV.Core.Models.InputModels;
using OmniePDV.Core.Models.ViewModels;

namespace OmniePDV.Api.Endpoints;

public class Manufacturers : ICarterModule
{
    const string ROUTE_PREFIX = "/api/v1";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(ROUTE_PREFIX + "/manufacturers/get-all", async (ISender sender) => 
        {
            List<Manufacturer> manufacturers = await sender.Send(new GetAllManufacturersQuery());

            if (manufacturers.Count == 0)
                return Results.NoContent();

            return Results.Ok(new ResultViewModel<List<ManufacturerViewModel>>
            {
                StatusCode = HttpStatusCode.OK,
                Data = manufacturers.ToViewModel()
            });
        });

        app.MapPost(ROUTE_PREFIX + "/manufacturers/add", async ([FromBody] ManufacturerInputModel body, ISender sender) =>
        {
            Manufacturer manufacturer = await sender.Send(new CreateManufacturerCommand(
                Name: body.Name,
                CRN: body.CRN,
                Active: body.Active
            ));

            return Results.Created(string.Empty, new ResultViewModel<ManufacturerViewModel>
            {
                StatusCode = HttpStatusCode.OK,
                Data = manufacturer.ToViewModel()
            });
        });

        app.MapPut(ROUTE_PREFIX + "/manufacturers/update/{id:guid}", async (Guid id, [FromBody] ManufacturerInputModel body, ISender sender) =>
        {
            Manufacturer manufacturer = await sender.Send(new UpdateManufacturerCommand(
                ID: id,
                Name: body.Name,
                CRN: body.CRN,
                Active: body.Active
            ));

            return Results.Ok(new ResultViewModel<ManufacturerViewModel>
            {
                StatusCode = HttpStatusCode.OK,
                Data = manufacturer.ToViewModel()
            });
        });

        app.MapDelete(ROUTE_PREFIX + "/manufacturers/delete/{id:guid}", async (Guid id, ISender sender) =>
        {
            await sender.Send(new DeleteManufacturerCommand(ID: id));

            return Results.NoContent();
        });
    }
}