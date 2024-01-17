using Microsoft.AspNetCore.Mvc;
using OmniePDV.API.Data.Entities;
using OmniePDV.API.Entities;
using OmniePDV.API.Models.InputModels;
using OmniePDV.API.Models.ViewModels;
using OmniePDV.API.Models.ViewModels.Base;
using OmniePDV.API.Services.Interfaces;
using System.Net;

namespace OmniePDV.API.Controllers;

[Route("api/point-of-sales")]
[ApiController]
[Produces("application/json")]
public class PointOfSalesController(IPointOfSalesService pointOfSalesService) : ControllerBase
{
    private readonly IPointOfSalesService _pointOfSalesService = pointOfSalesService;

    [HttpGet("get-opened-sale")]
    public async Task<IActionResult> GetOpenSale()
    {
        Sale sale = await _pointOfSalesService.GetOpenedSaleAsync();

        if (sale == null)
            return NoContent();

        return Ok(JsonResultViewModel<POSViewModel>
            .New(HttpStatusCode.OK, sale.ToViewModel()));
    }

    [HttpPost("add-product")]
    public async Task<IActionResult> AddProductToSale([FromBody] AddProductToSaleInputModel body)
    {
        Sale sale = await _pointOfSalesService.AddProductToSaleAsync(body);

        return Ok(JsonResultViewModel<POSViewModel>
            .New(HttpStatusCode.OK, sale.ToViewModel()));
    }

    [HttpPut("change-opened-sale-status")]
    public async Task<IActionResult> CloseOpenedSale([FromBody] ChangeOpenedSaleStatusInputModel body)
    {
        Sale sale = await _pointOfSalesService.ChangeOpenedSaleStatusAsync(body);

        if (sale.Status == SaleStatusEnum.Open)
            return Ok(JsonResultViewModel<POSViewModel>
                .New(HttpStatusCode.OK, sale.ToViewModel()));
        else
            return NoContent();
    }

    [HttpPatch("add-discount")]
    public async Task<IActionResult> AddDiscountToSale([FromBody] AddDiscountToSaleInputModel body)
    {
        Sale sale = await _pointOfSalesService.AddDiscountToSaleAsync(body);

        return Ok(JsonResultViewModel<POSViewModel>
            .New(HttpStatusCode.OK, sale.ToViewModel()));
    }

    [HttpPatch("add-client")]
    public async Task<IActionResult> AddClientToSale([FromBody] AddClientToSaleInputModel body)
    {
        Sale sale = await _pointOfSalesService.AddClientToSaleAsync(body);

        return Ok(JsonResultViewModel<POSViewModel>
            .New(HttpStatusCode.OK, sale.ToViewModel()));
    }

    [HttpDelete("delete-product/{order:int}")]
    public async Task<IActionResult> DeleteProductFromSale([FromRoute] int order)
    {
        Sale sale = await _pointOfSalesService.DeleteProductFromSaleAsync(order);

        return Ok(JsonResultViewModel<POSViewModel>
            .New(HttpStatusCode.OK, sale.ToViewModel()));
    }
}
