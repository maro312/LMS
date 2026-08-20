using System.Text.Json.Serialization;
using LMS.Core.Enums;

namespace LMS.Core.Results;

public class Result<T> : IResult
{
    protected Result() { }

    public Result(T value)
    {
        Value = value;
        Status = ResultStatus.Ok;
    }

    protected internal Result(T value, string successMessage) : this(value)
    {
        SuccessMessage = successMessage;
    }

    public Result(T value, ResultStatus status) : this(value)
    {
        Status = status;
    }

    protected Result(ResultStatus status)
    {
        Status = status;
    }

    public static implicit operator T(Result<T> result) => result.Value;
    public static implicit operator Result<T>(T value) => new Result<T>(value);

    public static implicit operator Result<T>(Result result) => new Result<T>(default(T)!)
    {
        Status = result.Status,
        ErrorCode = result.ErrorCode,
        Errors = result.Errors,
        SuccessMessage = result.SuccessMessage,
        CorrelationId = result.CorrelationId,
        ValidationErrors = result.ValidationErrors,
    };

    public T Value { get; } = default!;

    [JsonIgnore]
    public Type ValueType => typeof(T);
    public ResultStatus Status { get; protected set; } = ResultStatus.Ok;
    public bool IsSuccess => Status == ResultStatus.Ok || Status == ResultStatus.Created;
    public string SuccessMessage { get; protected set; } = string.Empty;
    public string CorrelationId { get; protected set; } = string.Empty;
    public string ErrorCode { get; protected set; } = string.Empty;

    public string Code => !string.IsNullOrWhiteSpace(ErrorCode)
        ? ErrorCode
        : Status switch
        {
            ResultStatus.Ok => "SUCCESS",
            ResultStatus.Created => "CREATED",
            ResultStatus.Invalid => "VALIDATION_ERROR",
            ResultStatus.BadRequest => "BAD_REQUEST",
            ResultStatus.NotFound => "NOT_FOUND",
            ResultStatus.Unauthorized => "UNAUTHORIZED",
            ResultStatus.Forbidden => "FORBIDDEN",
            ResultStatus.Conflict => "CONFLICT",
            ResultStatus.InternalServerError => "INTERNAL_SERVER_ERROR",
            ResultStatus.Unavailable => "SERVICE_UNAVAILABLE",
            _ => "ERROR"
        };

    public IEnumerable<string> Errors { get; protected set; } = new List<string>();
    public List<ValidationError> ValidationErrors { get; protected set; } = new List<ValidationError>();

    public object GetValue()
    {
        return this.Value!;
    }

    public ErrorPayload ToErrorPayload(string? traceId = null)
    {
        var primaryMessage = Errors.FirstOrDefault();
        if (string.IsNullOrWhiteSpace(primaryMessage))
        {
            if (ValidationErrors.Any())
            {
                primaryMessage = "Validation failure occurred.";
            }
            else
            {
                primaryMessage = Status switch
                {
                    ResultStatus.NotFound => "The requested resource was not found.",
                    ResultStatus.Unauthorized => "Unauthorized access.",
                    ResultStatus.Forbidden => "Forbidden action.",
                    ResultStatus.Conflict => "A conflict occurred with the current state.",
                    ResultStatus.BadRequest => "Invalid request payload.",
                    ResultStatus.Invalid => "Business validation failed.",
                    ResultStatus.InternalServerError => "An unexpected backend error occurred.",
                    _ => "An error occurred."
                };
            }
        }

        var details = ValidationErrors.Select(v => v.ToErrorDetail()).ToList();
        var effectiveTraceId = !string.IsNullOrWhiteSpace(traceId) ? traceId : CorrelationId;

        return new ErrorPayload(Code, primaryMessage, details, effectiveTraceId);
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(value);
    }

    public static Result<T> Success(T value, string successMessage)
    {
        return new Result<T>(value, successMessage);
    }

    public static Result<T> Created(T value)
    {
        return new Result<T>(value, ResultStatus.Created);
    }

    public static Result<T> Created(T value, string successMessage)
    {
        return new Result<T>(value, successMessage) { Status = ResultStatus.Created };
    }

    public static Result<T> Error(params string[] errorMessages)
    {
        return new Result<T>(ResultStatus.Error) { Errors = errorMessages };
    }

    public static Result<T> ErrorWithCode(string code, params string[] errorMessages)
    {
        return new Result<T>(ResultStatus.Error) { ErrorCode = code, Errors = errorMessages };
    }

    public static Result<T> BadRequest(params string[] errorMessages)
    {
        return new Result<T>(ResultStatus.BadRequest) { Errors = errorMessages };
    }

    public static Result<T> Invalid(ValidationError validationError)
    {
        return new Result<T>(ResultStatus.Invalid) { ValidationErrors = { validationError } };
    }

    public static Result<T> Invalid(params ValidationError[] validationErrors)
    {
        return new Result<T>(ResultStatus.Invalid) { ValidationErrors = new List<ValidationError>(validationErrors) };
    }

    public static Result<T> Invalid(List<ValidationError> validationErrors)
    {
        return new Result<T>(ResultStatus.Invalid) { ValidationErrors = validationErrors };
    }

    public static Result<T> UnprocessableEntity(params ValidationError[] validationErrors)
    {
        return Invalid(validationErrors);
    }

    public static Result<T> NotFound()
    {
        return new Result<T>(ResultStatus.NotFound);
    }

    public static Result<T> NotFound(params string[] errorMessages)
    {
        return new Result<T>(ResultStatus.NotFound) { Errors = errorMessages };
    }

    public static Result<T> Forbidden()
    {
        return new Result<T>(ResultStatus.Forbidden);
    }

    public static Result<T> Forbidden(params string[] errorMessages)
    {
        return new Result<T>(ResultStatus.Forbidden) { Errors = errorMessages };
    }

    public static Result<T> Unauthorized()
    {
        return new Result<T>(ResultStatus.Unauthorized);
    }

    public static Result<T> Unauthorized(params string[] errorMessages)
    {
        return new Result<T>(ResultStatus.Unauthorized) { Errors = errorMessages };
    }

    public static Result<T> Conflict()
    {
        return new Result<T>(ResultStatus.Conflict);
    }

    public static Result<T> Conflict(params string[] errorMessages)
    {
        return new Result<T>(ResultStatus.Conflict) { Errors = errorMessages };
    }

    public static Result<T> InternalServerError(params string[] errorMessages)
    {
        return new Result<T>(ResultStatus.InternalServerError) { Errors = errorMessages };
    }

    public static Result<T> Unavailable(params string[] errorMessages)
    {
        return new Result<T>(ResultStatus.Unavailable) { Errors = errorMessages };
    }
}
