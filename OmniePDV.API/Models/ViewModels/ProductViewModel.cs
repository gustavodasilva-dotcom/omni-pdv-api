using OmniePDV.API.Data.Entities;
using System.Text.Json.Serialization;

namespace OmniePDV.API.Models.ViewModels;

public sealed class ProductViewModel
{
    [JsonPropertyName("id")]
    public Guid ID { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("wholesale_price")]
    public double WholesalePrice { get; set; }

    [JsonPropertyName("retail_price")]
    public double RetailPrice { get; set; }

    [JsonPropertyName("barcode")]
    public string Barcode { get; set; } = string.Empty;

    [JsonPropertyName("manufacturer")]
    public ManufacturerViewModel Manufacturer { get; set; }

    [JsonPropertyName("active")]
    public bool Active { get; set; }
}

public static class ProductViewModelExtensions
{
    public static ProductViewModel ToViewModel(this Product model) => new()
    {
        ID = model.UID,
        Name = model.Name,
        Description = model.Description,
        WholesalePrice = model.WholesalePrice,
        RetailPrice = model.RetailPrice,
        Barcode = model.Barcode,
        Manufacturer = model.Manufacturer.ToViewModel(),
        Active = model.Active
    };

    public static List<ProductViewModel> ToViewModel(this List<Product> model) =>
        model.Select(m => m.ToViewModel()).ToList();
}
