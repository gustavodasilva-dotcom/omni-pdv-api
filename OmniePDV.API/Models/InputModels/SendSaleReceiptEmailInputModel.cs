using System.Text.Json.Serialization;

namespace OmniePDV.API.Models.InputModels;

public sealed class SendSaleReceiptEmailInputModel
{
    [JsonPropertyName("sale_id")]
    public Guid SaleID { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }
}
