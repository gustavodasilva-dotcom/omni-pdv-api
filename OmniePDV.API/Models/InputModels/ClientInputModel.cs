using System.Text.Json.Serialization;

namespace OmniePDV.API.Models.InputModels;

public sealed class ClientInputModel
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("ssn")]
    public string SSN { get; set; }

    [JsonPropertyName("birthday")]
    public DateTime Birthday { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("active")]
    public bool Active { get; set; }
}
