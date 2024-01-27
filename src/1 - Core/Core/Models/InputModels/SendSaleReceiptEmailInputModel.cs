using OmniePDV.Core.Abstractions.Models;
using System.Text.Json.Serialization;

namespace OmniePDV.Core.Models.InputModels;

public sealed class SendSaleReceiptEmailInputModel : BaseInputModel
{
    [JsonPropertyName("sale_id")]
    public Guid SaleID { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    public override bool ValidateModel()
    {
        throw new NotImplementedException();
    }
}
