using System.Text.Json.Serialization;
using OmniePDV.Core.Abstractions.Models;
using OmniePDV.Core.Enumerations;

namespace OmniePDV.Core.Models.InputModels;

public sealed class AddDiscountToSaleInputModel : BaseInputModel
{
    [JsonPropertyName("type")]
    public DiscountTypeEnum Type { get; set; }

    [JsonPropertyName("value")]
    public double Value { get; set; }

    public override bool ValidateModel()
    {
        throw new NotImplementedException();
    }
}