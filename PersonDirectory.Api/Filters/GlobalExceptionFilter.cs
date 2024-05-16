using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PersonDirectory.Application.Shared.Resources;
using PersonDirectory.Shared;

namespace PersonDirectory.Api.Filters;

/// <inheritdoc/>
public class GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger) : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger = logger;

    public void OnException(ExceptionContext context)
    {
        _logger.LogError("Error Message: {ExceptionMessage}, occurred at: {Time}",
            context.Exception.Message, DateTime.UtcNow);

        var exception = context.Exception;
        var (detail, title, statusCode) = GetExceptionDetails(exception);

        var problemDetails = new ProblemDetails
        {
            Title = title,
            Detail = detail,
            Status = statusCode,
            Instance = context.HttpContext.Request.Path
        };

        problemDetails.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);

        if (exception is ValidationException validationException)
        {
            problemDetails.Extensions.Add("ValidationErrors", validationException.Errors);
        }

        context.HttpContext.Response.WriteAsJsonAsync(problemDetails).Wait();
        context.ExceptionHandled = true;
    }

    private static (string? Detail, string Title, int StatusCode) GetExceptionDetails(Exception exception)
    {
        return exception switch
        {
            ValidationException => (
                exception.Message,
                exception.GetType().Name,
                StatusCodes.Status400BadRequest
            ),
            AppException => (
                Resources.ResourceManager.GetString(exception.Message),
                exception.GetType().Name,
                StatusCodes.Status400BadRequest
            ),
            _ => (
                exception.Message,
                exception.GetType().Name,
                StatusCodes.Status500InternalServerError
            )
        };
    }
}
