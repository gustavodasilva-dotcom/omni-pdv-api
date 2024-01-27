using System.Net;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OmniePDV.Core.CQRS.PointOfSales.Commands.AddClientToSale;
using OmniePDV.Core.CQRS.PointOfSales.Commands.AddDiscountToSale;
using OmniePDV.Core.CQRS.PointOfSales.Commands.AddProductToSale;
using OmniePDV.Core.CQRS.PointOfSales.Commands.ChangeOpenedSaleStatus;
using OmniePDV.Core.CQRS.PointOfSales.Commands.DeleteProductFromSale;
using OmniePDV.Core.CQRS.PointOfSales.Commands.SendSaleReceiptToEmail;
using OmniePDV.Core.CQRS.PointOfSales.Queries.GetOpenedSale;
using OmniePDV.Core.Entities;
using OmniePDV.Core.Models.InputModels;
using OmniePDV.Core.Models.ViewModels;

namespace OmniePDV.Api.Endpoints;

public class PointOfSales : ICarterModule
{
    const string ROUTE_PREFIX = "/api/v1";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(ROUTE_PREFIX + "/point-of-sales/get-opened-sale", async (ISender sender) =>
        {
            Sale sale = await sender.Send(new GetOpenedSaleQuery());

            if (sale is null)
                return Results.NoContent();

            return Results.Ok(new ResultViewModel<POSViewModel>
            {
                StatusCode = HttpStatusCode.OK,
                Data = sale.ToViewModel()
            });
        });

        app.MapPost(ROUTE_PREFIX + "/point-of-sales/add-product", async ([FromBody] AddProductToSaleInputModel body, ISender sender) =>
        {
            Sale sale = await sender.Send(new AddProductToSaleCommand(
                Barcode: body.Barcode,
                Quantity: body.Quantity
            ));

            return Results.Ok(new ResultViewModel<POSViewModel>
            {
                StatusCode = HttpStatusCode.OK,
                Data = sale.ToViewModel()
            });
        });

        app.MapPut(ROUTE_PREFIX + "/point-of-sales/change-opened-sale-status", async ([FromBody] ChangeOpenedSaleStatusInputModel body, ISender sender) =>
        {
            Sale sale = await sender.Send(new ChangeOpenedSaleStatusCommand(Status: body.Status));

            return Results.Ok(new ResultViewModel<POSViewModel>
            {
                StatusCode = HttpStatusCode.OK,
                Data = sale.ToViewModel()
            });
        });

        app.MapPut(ROUTE_PREFIX + "/point-of-sales/send-receipt-email", async ([FromBody] SendSaleReceiptEmailInputModel body, ISender sender) =>
        {
            await sender.Send(new SendSaleReceiptToEmailCommand(
                SaleID: body.SaleID,
                Email: body.Email
            ));

            return Results.Ok();
        });

        app.MapPatch(ROUTE_PREFIX + "/point-of-sales/add-discount", async ([FromBody] AddDiscountToSaleInputModel body, ISender sender) =>
        {
            Sale sale = await sender.Send(new AddDiscountToSaleCommand(Type: body.Type, Value: body.Value));

            return Results.Ok(new ResultViewModel<POSViewModel>
            {
                StatusCode = HttpStatusCode.OK,
                Data = sale.ToViewModel()
            });
        });

        app.MapPatch(ROUTE_PREFIX + "/point-of-sales/add-client", async ([FromBody] AddClientToSaleInputModel body, ISender sender) =>
        {
            Sale sale = await sender.Send(new AddClientToSaleCommand(ClientID: body.ClientID));

            return Results.Ok(new ResultViewModel<POSViewModel>
            {
                StatusCode = HttpStatusCode.OK,
                Data = sale.ToViewModel()
            });
        });

        app.MapDelete(ROUTE_PREFIX + "/point-of-sales/delete-product/{order:int}", async (int order, ISender sender) =>
        {
            await sender.Send(new DeleteProductFromSaleCommand(Order: order));

            return Results.Ok();
        });
    }
}