using LMS.Core.Enums;

namespace LMS.Core.Results;

public interface IResult
{
    ResultStatus Status { get; }
    string Code { get; }
    IEnumerable<string> Errors { get; }
    List<ValidationError> ValidationErrors { get; }
    Type ValueType { get; }
    bool IsSuccess { get; }
    object GetValue();
    ErrorPayload ToErrorPayload(string? traceId = null);
}
