using System.Net;
using LMS.Core.Results;

namespace LMS.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "An unexpected error occurred.");

        var (statusCode, code, message) = exception switch
        {
            ApplicationException appEx => (HttpStatusCode.BadRequest, "BAD_REQUEST", appEx.Message),
            KeyNotFoundException _ => (HttpStatusCode.NotFound, "NOT_FOUND", "The requested resource was not found."),
            UnauthorizedAccessException _ => (HttpStatusCode.Unauthorized, "UNAUTHORIZED", "Unauthorized access."),
            _ => (HttpStatusCode.InternalServerError, "INTERNAL_SERVER_ERROR", "Internal server error. Please retry later.")
        };

        var traceId = context.TraceIdentifier;
        var errorPayload = new ErrorPayload(code, message, new List<ErrorDetail>(), traceId);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        await context.Response.WriteAsJsonAsync(errorPayload);
    }
}
