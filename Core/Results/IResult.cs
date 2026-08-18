using LMS.Core.Enums;

namespace LMS.Core.Results;

public interface IResult
{
    ResultStatus Status { get; }
    IEnumerable<string> Errors { get; }
    List<ValidationError> ValidationErrors { get; }
    Type ValueType { get; }
    object GetValue();
}
