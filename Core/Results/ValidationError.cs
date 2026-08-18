using System.Text.Json.Serialization;

namespace LMS.Core.Results;

public class ValidationError
{
    public ValidationError()
    {
    }

    public ValidationError(string message)
    {
        Message = message;
    }

    public ValidationError(string field, string message)
    {
        Field = field;
        Message = message;
    }

    public ValidationError(string identifier, string errorMessage, string errorCode, ValidationSeverity severity)
    {
        Field = identifier;
        Message = errorMessage;
        ErrorCode = errorCode;
        Severity = severity;
    }

    [JsonPropertyName("field")]
    public string Field { get; set; } = string.Empty;

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonIgnore]
    public string Identifier
    {
        get => Field;
        set => Field = value;
    }

    [JsonIgnore]
    public string ErrorMessage
    {
        get => Message;
        set => Message = value;
    }

    [JsonIgnore]
    public string ErrorCode { get; set; } = string.Empty;

    [JsonIgnore]
    public ValidationSeverity Severity { get; set; } = ValidationSeverity.Error;

    public ErrorDetail ToErrorDetail()
    {
        return new ErrorDetail(Field, Message);
    }
}
