using System.Text.Json.Serialization;

namespace OmniePDV.API.Models.InputModels;

public sealed class ProductInputModel
{
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

    [JsonPropertyName("manufacturer_id")]
    public Guid ManufacturerID { get; set; }

    [JsonPropertyName("active")]
    public bool Active { get; set; } = true;
}
