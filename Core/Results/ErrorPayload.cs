using System.Text.Json.Serialization;

namespace LMS.Core.Results;

public class ErrorPayload
{
    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("details")]
    public List<ErrorDetail> Details { get; set; } = new();

    [JsonPropertyName("traceId")]
    public string TraceId { get; set; } = string.Empty;

    public ErrorPayload()
    {
    }

    public ErrorPayload(string code, string message, IEnumerable<ErrorDetail>? details = null, string? traceId = null)
    {
        Code = code;
        Message = message;
        if (details != null)
        {
            Details.AddRange(details);
        }
        TraceId = traceId ?? string.Empty;
    }
}
