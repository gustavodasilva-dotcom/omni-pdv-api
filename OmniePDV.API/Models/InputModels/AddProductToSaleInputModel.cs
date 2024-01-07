using System.Text.Json.Serialization;

namespace OmniePDV.API.Models.InputModels;

public sealed class AddProductToSaleInputModel
{
    [JsonPropertyName("barcode")]
    public string Barcode { get; set; } = string.Empty;

    [JsonPropertyName("quantity")]
    public double Quantity { get; set; }
}
