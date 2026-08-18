using LMS.Core.Enums;
using LMS.Core.Results;
using Microsoft.AspNetCore.Mvc;
using CoreIResult = LMS.Core.Results.IResult;

namespace LMS.API.Extensions;

public static class ResultActionResultExtensions
{
    public static ActionResult ToActionResult<T>(this Result<T> result, ControllerContext? context = null)
    {
        return ConvertToActionResult(result, result.Value, context);
    }

    public static ActionResult ToActionResult(this Result result, ControllerContext? context = null)
    {
        return ConvertToActionResult(result, null, context);
    }

    private static ActionResult ConvertToActionResult(CoreIResult result, object? value, ControllerContext? context)
    {
        var statusCode = (int)result.Status;
        var traceId = context?.HttpContext.TraceIdentifier ?? string.Empty;

        if (result.IsSuccess)
        {
            if (result.Status == ResultStatus.Created)
            {
                return new ObjectResult(value) { StatusCode = 201 };
            }

            if (value == null && result.ValueType == typeof(Result))
            {
                return new OkResult();
            }

            return new OkObjectResult(value);
        }

        var errorPayload = result.ToErrorPayload(traceId);
        return new ObjectResult(errorPayload) { StatusCode = statusCode };
    }
}
