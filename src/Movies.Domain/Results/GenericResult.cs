using System.Net;
using System.Text.Json.Serialization;

namespace Movies.Domain.Results;

public class GenericResult
{
    public GenericResult(bool success,
                         HttpStatusCode status,
                         object? data,
                         IList<string>? messages = null)
    {
        Success = success;
        Status = status;
        Data = data;
        Messages = messages ?? [];
    }

    [JsonPropertyName("success")]
    public bool Success { get; private set; }

    [JsonPropertyName("status")]
    public HttpStatusCode Status { get; private set; }

    [JsonPropertyName("data")]
    public object? Data { get; private set; }
    [JsonPropertyName("messages")]
    public IList<string> Messages { get; private set; } = [];
}
