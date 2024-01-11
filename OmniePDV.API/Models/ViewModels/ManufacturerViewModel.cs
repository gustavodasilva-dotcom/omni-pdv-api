using OmniePDV.API.Data.Entities;
using System.Text.Json.Serialization;

namespace OmniePDV.API.Models.ViewModels;

public sealed class ManufacturerViewModel
{
    [JsonPropertyName("id")]
    public Guid ID { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("crn")]
    public string CRN { get; set; } = string.Empty;

    [JsonPropertyName("active")]
    public bool Active { get; set; }
}

public static class ManufacturerViewModelExtensions
{
    public static ManufacturerViewModel ToViewModel(this Manufacturer model) => new()
    {
        ID = model.UID,
        Name = model.Name,
        CRN = model.CRN,
        Active = model.Active
    };

    public static List<ManufacturerViewModel> ToViewModel(this List<Manufacturer> model) =>
        model.Select(m => m.ToViewModel()).ToList();
}
