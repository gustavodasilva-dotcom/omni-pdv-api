using OmniePDV.API.Entities;
using System.Text.Json.Serialization;

namespace OmniePDV.API.Models.InputModels;

public sealed class ChangeOpenedSaleStatusInputModel
{
    [JsonPropertyName("status")]
    public SaleStatusEnum Status { get; set; }
}
