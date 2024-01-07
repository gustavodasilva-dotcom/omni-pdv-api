using System.Text.Json.Serialization;

namespace OmniePDV.API.Models.InputModels;

public sealed class ManufacturerInputModel
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("crn")]
    public string CRN { get; set; } = string.Empty;

    [JsonPropertyName("active")]
    public bool Active { get; set; } = true;
}
