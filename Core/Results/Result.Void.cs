using LMS.Core.Enums;

namespace LMS.Core.Results;

public class Result : Result<Result>
{
    public Result() : base() { }

    protected internal Result(ResultStatus status) : base(status) { }

    public static Result Success()
    {
        return new Result();
    }

    public static Result SuccessWithMessage(string successMessage)
    {
        return new Result() { SuccessMessage = successMessage };
    }

    public static Result<T> Success<T>(T value)
    {
        return new Result<T>(value);
    }

    public static Result<T> Success<T>(T value, string successMessage)
    {
        return new Result<T>(value, successMessage);
    }

    public new static Result Error(params string[] errorMessages)
    {
        return new Result(ResultStatus.Error) { Errors = errorMessages };
    }

    public static Result ErrorWithCorrelationId(string correlationId, params string[] errorMessages)
    {
        return new Result(ResultStatus.Error)
        {
            CorrelationId = correlationId,
            Errors = errorMessages
        };
    }

    public new static Result Invalid(ValidationError validationError)
    {
        return new Result(ResultStatus.Invalid) { ValidationErrors = { validationError } };
    }

    public new static Result Invalid(params ValidationError[] validationErrors)
    {
        return new Result(ResultStatus.Invalid) { ValidationErrors = new List<ValidationError>(validationErrors) };
    }

    public new static Result Invalid(List<ValidationError> validationErrors)
    {
        return new Result(ResultStatus.Invalid) { ValidationErrors = validationErrors };
    }

    public new static Result NotFound()
    {
        return new Result(ResultStatus.NotFound);
    }

    public new static Result NotFound(params string[] errorMessages)
    {
        return new Result(ResultStatus.NotFound) { Errors = errorMessages };
    }

    public new static Result Forbidden()
    {
        return new Result(ResultStatus.Forbidden);
    }

    public new static Result Unauthorized()
    {
        return new Result(ResultStatus.Unauthorized);
    }

    public new static Result Conflict()
    {
        return new Result(ResultStatus.Conflict);
    }

    public new static Result Conflict(params string[] errorMessages)
    {
        return new Result(ResultStatus.Conflict) { Errors = errorMessages };
    }

    public new static Result Unavailable(params string[] errorMessages)
    {
        return new Result(ResultStatus.Unavailable) { Errors = errorMessages };
    }

    public new static Result CriticalError(params string[] errorMessages)
    {
        return new Result(ResultStatus.CriticalError) { Errors = errorMessages };
    }
}
