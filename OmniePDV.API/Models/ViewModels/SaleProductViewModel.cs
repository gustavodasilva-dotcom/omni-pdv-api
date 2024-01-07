using OmniePDV.API.Data.Entities;
using System.Text.Json.Serialization;

namespace OmniePDV.API.Models.ViewModels;

public sealed class SaleProductViewModel
{
    [JsonPropertyName("uid")]
    public Guid UID { get; set; }

    [JsonPropertyName("quantity")]
    public double Quantity { get; set; }

    [JsonPropertyName("product")]
    public ProductViewModel Product { get; set; }
}

public static class SaleProductViewModelExtensions
{
    public static SaleProductViewModel ToViewModel(this SaleProduct model) => new()
    {
        UID = model.UID,
        Quantity = model.Quantity,
        Product = model.Product.ToViewModel()
    };

    public static List<SaleProductViewModel> ToViewModel(this List<SaleProduct> model) =>
        model.Select(m => m.ToViewModel()).ToList();
}
