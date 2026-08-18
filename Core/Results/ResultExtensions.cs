using LMS.Core.Enums;

namespace LMS.Core.Results;

public static class ResultExtensions
{
    public static Result<TDestination> Map<TSource, TDestination>(this Result<TSource> result, Func<TSource, TDestination> func)
    {
        switch (result.Status)
        {
            case ResultStatus.Ok: return func(result);
            case ResultStatus.NotFound:
                return result.Errors.Any()
                    ? Result<TDestination>.NotFound(result.Errors.ToArray())
                    : Result<TDestination>.NotFound();
            case ResultStatus.Unauthorized: return Result<TDestination>.Unauthorized();
            case ResultStatus.Forbidden: return Result<TDestination>.Forbidden();
            case ResultStatus.Invalid: return Result<TDestination>.Invalid(result.ValidationErrors);
            case ResultStatus.Error: return Result<TDestination>.Error(result.Errors.ToArray());
            case ResultStatus.Conflict:
                return result.Errors.Any()
                    ? Result<TDestination>.Conflict(result.Errors.ToArray())
                    : Result<TDestination>.Conflict();
            case ResultStatus.CriticalError: return Result<TDestination>.CriticalError(result.Errors.ToArray());
            case ResultStatus.Unavailable: return Result<TDestination>.Unavailable(result.Errors.ToArray());
            default:
                throw new NotSupportedException($"Results {result.Status} conversion is not supported.");
        }
    }
}
