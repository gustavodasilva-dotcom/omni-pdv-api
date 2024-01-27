using OmniePDV.Core.Entities;
using OmniePDV.Core.Enumerations;
using System.Text.Json.Serialization;

namespace OmniePDV.Core.Models.ViewModels;

public sealed class DiscountViewModel
{
    [JsonPropertyName("type")]
    public DiscountTypeEnum Type { get; set; }

    [JsonPropertyName("value")]
    public double Value { get; set; }
}

public static class DiscountViewModelExtensions
{
    public static DiscountViewModel ToViewModel(this Discount model) => new()
    {
        Type = model.Type,
        Value = model.Value
    };

    public static List<DiscountViewModel> ToViewModel(this List<Discount> model) =>
        model.Select(m => m.ToViewModel()).ToList();
}