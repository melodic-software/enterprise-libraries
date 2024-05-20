using Enterprise.Serialization.Json.Microsoft;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System.Text.Json;
using Enterprise.Validation.Exceptions;
using Enterprise.Validation.Extensions;

namespace Enterprise.Api.ErrorHandling.ExceptionHandlers;

public class ValidationExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ValidationException validationException)
        {
            return false;
        }

        httpContext.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;

        var validationErrors = validationException.ValidationErrors.ToList();

        if (!validationErrors.Any())
        {
            return true;
        }

        var dictionary = validationErrors.ToDictionary();

        // TODO: Support alternative content types like XML
        // Right now we're just supporting JSON even if a client asks for something else.

        //StringValues acceptHeader = httpContext.Request.Headers.Accept;

        //bool mediaTypeParsed = MediaTypeHeaderValue
        //    .TryParse(acceptHeader.ToString(), out MediaTypeHeaderValue? parsedMediaType);

        //// TODO: Use constants instead of magic strings.
        //bool useJson = parsedMediaType != null && (parsedMediaType.Type == "*" || parsedMediaType.Type == "json");

        //if (useJson)
        //{
        //    await CreateJsonResponseAsync(httpContext, dictionary, cancellationToken);
        //}
        //else
        //{
        //    // TODO: Serialize to XML and set Content-Type appropriately (application/xml)
        //    // For now, we're just going to return JSON.
        //    await CreateJsonResponseAsync(httpContext, dictionary, cancellationToken);
        //}

        await CreateJsonResponseAsync(httpContext, dictionary, cancellationToken);

        return true;
    }

    private static async Task CreateJsonResponseAsync(HttpContext httpContext, Dictionary<string, string[]> dictionary, CancellationToken cancellationToken)
    {
        JsonSerializerOptions serializerOptions = JsonSerializerOptionsService.GetDefaultOptions();
        await httpContext.Response.WriteAsJsonAsync(dictionary, serializerOptions, cancellationToken: cancellationToken);
    }
}
