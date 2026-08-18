using System.Text.Json.Serialization;

namespace LMS.Core.Results;

public class ErrorDetail
{
    [JsonPropertyName("field")]
    public string Field { get; set; } = string.Empty;

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    public ErrorDetail()
    {
    }

    public ErrorDetail(string field, string message)
    {
        Field = field;
        Message = message;
    }
}
