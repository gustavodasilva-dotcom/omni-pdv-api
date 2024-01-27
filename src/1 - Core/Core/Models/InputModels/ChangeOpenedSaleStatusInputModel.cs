using OmniePDV.Core.Abstractions.Models;
using OmniePDV.Core.Enumerations;
using System.Text.Json.Serialization;

namespace OmniePDV.Core.Models.InputModels;

public sealed class ChangeOpenedSaleStatusInputModel : BaseInputModel
{
    [JsonPropertyName("status")]
    public SaleStatusEnum Status { get; set; }

    public override bool ValidateModel()
    {
        throw new NotImplementedException();
    }
}
