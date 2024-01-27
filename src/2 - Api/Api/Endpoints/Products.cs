using System.Net;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OmniePDV.Core.CQRS.Products.Commands.CreateProduct;
using OmniePDV.Core.CQRS.Products.Commands.DeleteProduct;
using OmniePDV.Core.CQRS.Products.Commands.UpdateProduct;
using OmniePDV.Core.CQRS.Products.Queries.GetAllProducts;
using OmniePDV.Core.CQRS.Products.Queries.GetProductByBarcode;
using OmniePDV.Core.Entities;
using OmniePDV.Core.Models.InputModels;
using OmniePDV.Core.Models.ViewModels;

namespace OmniePDV.Api.Endpoints;

public class Products : ICarterModule
{
    const string ROUTE_PREFIX = "/api/v1";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(ROUTE_PREFIX + "/products/get-all", async (ISender sender) =>
        {
            List<Product> products = await sender.Send(new GetAllProductsQuery());

            if (products.Count == 0)
                return Results.NoContent();

            return Results.Ok(new ResultViewModel<List<ProductViewModel>>
            {
                StatusCode = HttpStatusCode.OK,
                Data = products.ToViewModel()
            });
        });

        app.MapGet(ROUTE_PREFIX + "/products/get-by-barcode/{barcode}", async (string barcode, ISender sender) =>
        {
            Product product = await sender.Send(new GetProductByBarcodeQuery(Barcode: barcode));

            return Results.Ok(new ResultViewModel<ProductViewModel>
            {
                StatusCode = HttpStatusCode.OK,
                Data = product.ToViewModel()
            });
        });

        app.MapPost(ROUTE_PREFIX + "/products/add", async ([FromBody] ProductInputModel body, ISender sender) =>
        {
            Product product = await sender.Send(new CreateProductCommand(
                Name: body.Name,
                Description: body.Description,
                WholesalePrice: body.WholesalePrice,
                RetailPrice: body.RetailPrice,
                Barcode: body.Barcode,
                ManufacturerID: body.ManufacturerID,
                Active: body.Active
            ));

            return Results.Created(string.Empty, new ResultViewModel<ProductViewModel>
            {
                StatusCode = HttpStatusCode.Created,
                Data = product.ToViewModel()
            });
        });

        app.MapPut(ROUTE_PREFIX + "/products/update/{id:guid}", async (Guid id, [FromBody] ProductInputModel body, ISender sender) =>
        {
            Product product = await sender.Send(new UpdateProductCommand(
                ID: id,
                Name: body.Name,
                Description: body.Description,
                WholesalePrice: body.WholesalePrice,
                RetailPrice: body.RetailPrice,
                Barcode: body.Barcode,
                ManufacturerID: body.ManufacturerID,
                Active: body.Active
            ));

            return Results.Ok(new ResultViewModel<ProductViewModel>
            {
                StatusCode = HttpStatusCode.OK,
                Data = product.ToViewModel()
            });
        });

        app.MapDelete(ROUTE_PREFIX + "/products/delete/{id:guid}", async (Guid id, ISender sender) =>
        {
            await sender.Send(new DeleteProductCommand(ID: id));

            return Results.NoContent();
        });
    }
}