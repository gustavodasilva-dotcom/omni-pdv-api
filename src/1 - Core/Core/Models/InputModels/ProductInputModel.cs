using OmniePDV.Core.Abstractions.Models;
using System.Text.Json.Serialization;

namespace OmniePDV.Core.Models.InputModels;

public sealed class ProductInputModel : BaseInputModel
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

    public override bool ValidateModel()
    {
        throw new NotImplementedException();
    }
}
