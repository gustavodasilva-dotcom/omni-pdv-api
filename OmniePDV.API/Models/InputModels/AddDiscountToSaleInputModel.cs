using System.Text.Json.Serialization;
using OmniePDV.API.Entities;

namespace OmniePDV.API.Models.InputModels;

public sealed class AddDiscountToSaleInputModel
{
    [JsonPropertyName("type")]
    public DiscountTypeEnum Type { get; set; }

    [JsonPropertyName("value")]
    public double Value { get; set; }
}