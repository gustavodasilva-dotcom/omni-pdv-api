using System.Text.Json.Serialization;

namespace OmniePDV.API.Models.InputModels;

public sealed class AddClientToSaleInputModel
{
    [JsonPropertyName("client_id")]
    public Guid ClientID { get; set; }
}