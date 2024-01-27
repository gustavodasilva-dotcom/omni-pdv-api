using OmniePDV.Core.Abstractions.Models;
using System.Text.Json.Serialization;

namespace OmniePDV.Core.Models.InputModels;

public sealed class AddProductToSaleInputModel : BaseInputModel
{
    [JsonPropertyName("barcode")]
    public string Barcode { get; set; } = string.Empty;

    [JsonPropertyName("quantity")]
    public double Quantity { get; set; }

    public override bool ValidateModel()
    {
        throw new NotImplementedException();
    }
}
