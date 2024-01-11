using System.Net;
using System.Text.Json.Serialization;

namespace OmniePDV.API.Models.ViewModels.Base;

public sealed record JsonResultViewModel<T> where T : class
{
    [JsonPropertyName("status_code")]
    public HttpStatusCode StatusCode { get; set; }

    [JsonPropertyName("data")]
    public T Data { get; set; }

    public static JsonResultViewModel<T> New(HttpStatusCode statusCode, T data) => new()
    {
        StatusCode = statusCode,
        Data = data
    };
}
