using OmniePDV.Core.Abstractions.Models;
using System.Text.Json.Serialization;

namespace OmniePDV.Core.Models.InputModels;

public sealed class ManufacturerInputModel : BaseInputModel
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("crn")]
    public string CRN { get; set; } = string.Empty;

    [JsonPropertyName("active")]
    public bool Active { get; set; } = true;

    public override bool ValidateModel()
    {
        throw new NotImplementedException();
    }
}
