using OmniePDV.API.Data.Entities;
using System.Text.Json.Serialization;

namespace OmniePDV.API.Models.ViewModels;

public sealed class POSProductViewModel
{
    [JsonPropertyName("id")]
    public Guid ID { get; set; }

    [JsonPropertyName("order")]
    public int Order { get; set; }

    [JsonPropertyName("quantity")]
    public double Quantity { get; set; }

    [JsonPropertyName("product")]
    public ProductViewModel Product { get; set; }

    [JsonPropertyName("deleted")]
    public bool Deleted { get; set; }
}

public static class SaleProductViewModelExtensions
{
    public static POSProductViewModel ToViewModel(this SaleProduct model) => new()
    {
        ID = model.UID,
        Order = model.Order,
        Quantity = model.Quantity,
        Product = model.Product.ToViewModel(),
        Deleted = model.Deleted
    };

    public static List<POSProductViewModel> ToViewModel(this List<SaleProduct> model) =>
        model.Select(m => m.ToViewModel()).ToList();
}
