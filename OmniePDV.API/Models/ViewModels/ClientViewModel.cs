using OmniePDV.API.Data.Entities;
using System.Text.Json.Serialization;

namespace OmniePDV.API.Models.ViewModels;

public sealed class ClientViewModel
{
    [JsonPropertyName("id")]
    public Guid ID { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("ssn")]
    public string SSN { get; set; }

    [JsonPropertyName("birthday")]
    public DateTime Birthday { get; set; }

    [JsonPropertyName("active")]
    public bool Active { get; set; }
}

public static class ClientViewModelExtensions
{
    public static ClientViewModel ToViewModel(this Client model) => new()
    {
        ID = model.UID,
        Name = model.Name,
        SSN = model.SSN,
        Birthday = model.Birthday,
        Active = model.Active
    };

    public static List<ClientViewModel> ToViewModel(this List<Client> model) =>
        model.Select(m => m.ToViewModel()).ToList();
}
