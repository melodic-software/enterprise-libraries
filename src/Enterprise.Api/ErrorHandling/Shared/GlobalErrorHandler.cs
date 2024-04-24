using System.Net.Mime;
using System.Text.Json;
using Enterprise.Api.ErrorHandling.Constants;
using Enterprise.Api.ErrorHandling.Dtos;
using Enterprise.Exceptions;
using Enterprise.Logging.Core.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Enterprise.Api.ErrorHandling.Shared;

[Obsolete("Use IExceptionHandler instead of middleware. This was introduced with .NET 8.")]
public class GlobalErrorHandler
{
    internal const string ErrorMessage = "Something went wrong.";

    internal static async Task HandleError(HttpContext context, Exception exception, ILogger? logger)
    {
        if (context.Response.HasStarted)
            return;

        // These are two separate methods for handling the error.
        // Since exception handlers or the Hellang middleware is more likely to be used I left this as is.

        //await HandleError(context, exception, logger);
        await HandleProblemDetails(context, exception);
    }

    private static async Task UseSimpleError(HttpContext context, Exception exception, ILogger logger)
    {
        // TODO: Provide functionality to use JSON or XML depending on the request header.

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = MediaTypeNames.Application.Json;

        JsonSerializerDefaults serializationDefaults = JsonSerializerDefaults.Web;
        JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions(serializationDefaults);

        // This is an optional numeric ID and name that can be used when logging that represents the "type" of event.
        EventId eventId;

        ErrorDetailsDto errorDetailsDto = new ErrorDetailsDto(context.Response.StatusCode, ErrorMessage);

        string json = JsonSerializer.Serialize(errorDetailsDto, jsonSerializerOptions);

        if (exception is HttpRequestException httpRequestException)
        {
            if (httpRequestException.StatusCode.HasValue)
                context.Response.StatusCode = (int)httpRequestException.StatusCode.Value;

            // TODO: The result objects / exceptions might have additional properties.

            await context.Response.WriteAsync(json);

            eventId = LogEventIds.CustomError;
        }
        else
        {
            await context.Response.WriteAsync(json);

            eventId = LogEventIds.UnknownError;
        }

        logger?.LogError(eventId, exception, exception.Message);
    }

    private static async Task HandleProblemDetails(HttpContext context, Exception exception)
    {
        ExceptionDetails exceptionDetails = GetExceptionDetails(exception);

        ProblemDetails problemDetails = new ProblemDetails
        {
            Status = exceptionDetails.Status,
            Type = exceptionDetails.Type,
            Title = exceptionDetails.Title,
            Detail = exceptionDetails.Detail
        };

        if (exceptionDetails.Errors is not null)
            problemDetails.Extensions[ProblemDetailsConstants.ErrorsExtensionKey] = exceptionDetails.Errors;

        context.Response.StatusCode = exceptionDetails.Status;

        await context.Response.WriteAsJsonAsync(problemDetails);
    }

    private static ExceptionDetails GetExceptionDetails(Exception exception)
    {
        return exception switch
        {
            ValidationException validationException => new ExceptionDetails(
                StatusCodes.Status422UnprocessableEntity,
                ValidationProblemDetailsConstants.Link,
                ValidationProblemDetailsConstants.Title,
                ValidationProblemDetailsConstants.Detail,
                validationException.ValidationErrors
            ),
            _ => new ExceptionDetails(
                StatusCodes.Status500InternalServerError,
                ProblemDetailsConstants.Type,
                ProblemDetailsConstants.Title,
                ProblemDetailsConstants.Detail,
                null
            )
        };
    }
}

internal record ExceptionDetails
{
    public ExceptionDetails(int status, string type, string title, string detail, IEnumerable<object>? errors)
    {
        Status = status;
        Type = type;
        Title = title;
        Detail = detail;
        Errors = errors;
    }

    public int Status { get; init; }
    public string Type { get; init; }
    public string Title { get; init; }
    public string Detail { get; init; }
    public IEnumerable<object>? Errors { get; init; }
}