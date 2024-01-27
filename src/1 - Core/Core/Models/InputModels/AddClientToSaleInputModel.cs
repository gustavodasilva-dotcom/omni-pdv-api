using OmniePDV.Core.Abstractions.Models;
using System.Text.Json.Serialization;

namespace OmniePDV.Core.Models.InputModels;

public sealed class AddClientToSaleInputModel : BaseInputModel
{
    [JsonPropertyName("client_id")]
    public Guid ClientID { get; set; }

    public override bool ValidateModel()
    {
        throw new NotImplementedException();
    }
}