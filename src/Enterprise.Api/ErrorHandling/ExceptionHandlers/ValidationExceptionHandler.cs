using Enterprise.Exceptions;
using Enterprise.Serialization.Json.Microsoft;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System.Text.Json;
using Enterprise.Validation.Extensions;
using Enterprise.Validation.Model;

namespace Enterprise.Api.ErrorHandling.ExceptionHandlers;

public class ValidationExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ValidationException validationException)
            return false;

        httpContext.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;

        List<ValidationError> validationErrors = validationException.ValidationErrors.ToList();

        if (!validationErrors.Any())
            return true;

        Dictionary<string, string[]> dictionary = validationErrors.ToDictionary();

        StringValues acceptHeader = httpContext.Request.Headers.Accept;

        bool mediaTypeParsed = MediaTypeHeaderValue.TryParse(acceptHeader.ToString(), out MediaTypeHeaderValue? parsedMediaType);

        // TODO: Use constants instead of magic strings.
        bool useJson = !mediaTypeParsed || parsedMediaType?.Type == "*" || parsedMediaType?.Type == "json";

        if (useJson)
        {
            await CreateJsonResponseAsync(httpContext, cancellationToken, dictionary);
        }
        else
        {
            // TODO: Serialize to XML and set Content-Type appropriately (application/xml)
            // For now, we're just going to return JSON.
            await CreateJsonResponseAsync(httpContext, cancellationToken, dictionary);
        }

        return true;
    }

    private static async Task CreateJsonResponseAsync(HttpContext httpContext, CancellationToken cancellationToken, Dictionary<string, string[]> dictionary)
    {
        JsonSerializerOptions serializerOptions = JsonSerializerOptionsService.GetDefaultOptions();
        await httpContext.Response.WriteAsJsonAsync(dictionary, serializerOptions, cancellationToken: cancellationToken);
    }
}
