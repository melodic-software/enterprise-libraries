using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;

namespace Enterprise.Api.Minimal.RouteHandling;

public static class RouteHandlerBuilderExtensions
{
    public static RouteHandlerBuilder ProducesStandard(this RouteHandlerBuilder builder, bool requiresAuthorization = true)
    {
        // This maps to a 400 Bad Request with validation problem details.
        // We don't have model binding validation for minimal APIs.
        // This can be returned if the command/query validation fails.
        builder.ProducesValidationProblem();

        // Ideally, we'd inspect the route builder to see if .RequiresAuthorization() has been called, but we don't seem to have access to it...
        // For now, we're going to default this since everything should be locked down by default and use .AllowAnonymous() if needed.
        if (requiresAuthorization)
        {
            builder.Produces(StatusCodes.Status401Unauthorized);
        }

        return builder
            .Produces(StatusCodes.Status406NotAcceptable)
            .ProducesValidationProblem(StatusCodes.Status422UnprocessableEntity)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithOpenApi(StandardOperations);
    }

    private static OpenApiOperation StandardOperations(OpenApiOperation operation)
    {
        if (operation.Responses.TryGetValue(StatusCodes.Status400BadRequest.ToString(CultureInfo.InvariantCulture), out OpenApiResponse? badRequestResponse))
        {
            badRequestResponse.Description = "Request is invalid.";
        }

        if (operation.Responses.TryGetValue(StatusCodes.Status401Unauthorized.ToString(CultureInfo.InvariantCulture), out OpenApiResponse? unauthorizedResponse))
        {
            unauthorizedResponse.Description = "Not authorized";
        }

        if (operation.Responses.TryGetValue(StatusCodes.Status406NotAcceptable.ToString(CultureInfo.InvariantCulture), out OpenApiResponse? notAcceptableResponse))
        {
            notAcceptableResponse.Description = "Cannot produce a response matching the request.";
        }

        if (operation.Responses.TryGetValue(StatusCodes.Status415UnsupportedMediaType.ToString(CultureInfo.InvariantCulture), out OpenApiResponse? unsupportedMediaTypeResponse))
        {
            unsupportedMediaTypeResponse.Description = "Media type not supported.";
        }

        if (operation.Responses.TryGetValue(StatusCodes.Status422UnprocessableEntity.ToString(CultureInfo.InvariantCulture), out OpenApiResponse? unprocessableEntityResponse))
        {
            unprocessableEntityResponse.Description = "The request cannot be processed.";
        }

        if (operation.Responses.TryGetValue(StatusCodes.Status500InternalServerError.ToString(CultureInfo.InvariantCulture), out OpenApiResponse? internalServerErrorResponse))
        {
            internalServerErrorResponse.Description = "An error occurred while processing the request.";
        }

        return operation;
    }
}
