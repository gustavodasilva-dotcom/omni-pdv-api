using System.Net;
using System.Text.Json.Serialization;

namespace OmniePDV.Core.Models.ViewModels;

public sealed record ResultViewModel<T> where T : class
{
    [JsonPropertyName("status_code")]
    public HttpStatusCode StatusCode { get; set; }

    [JsonPropertyName("data")]
    public T Data { get; set; }
}
