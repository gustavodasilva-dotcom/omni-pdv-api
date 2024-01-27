using OmniePDV.Core.Entities;
using OmniePDV.Core.Enumerations;
using System.Text.Json.Serialization;

namespace OmniePDV.Core.Models.ViewModels;

public sealed class POSViewModel
{
    [JsonPropertyName("id")]
    public Guid ID { get; set; }

    [JsonPropertyName("number")]
    public long Number { get; set; }

    [JsonPropertyName("subtotal")]
    public double Subtotal { get; set; }

    [JsonPropertyName("discount")]
    public DiscountViewModel? Discount { get; set; }

    [JsonPropertyName("total")]
    public double Total { get; set; }

    [JsonPropertyName("client")]
    public ClientViewModel Client { get; set; }

    [JsonPropertyName("products")]
    public List<POSProductViewModel> Products { get; set; }

    [JsonPropertyName("status")]
    public SaleStatusEnum Status { get; set; }

    [JsonPropertyName("sale_date")]
    public DateTime SaleDate { get; set; }
}

public static class SaleViewModelExtensions
{
    public static POSViewModel ToViewModel(this Sale model) => new()
    {
        ID = model.UID,
        Number = model.Number,
        Subtotal = model.Subtotal,
        Discount = model.Discount?.ToViewModel(),
        Total = model.Total,
        Client = model.Client.ToViewModel(),
        Products = model.Products.ToViewModel(),
        Status = model.Status,
        SaleDate = model.SaleDate
    };

    public static List<POSViewModel> ToViewModel(this List<Sale> model) =>
        model.Select(m => m.ToViewModel()).ToList();
}
