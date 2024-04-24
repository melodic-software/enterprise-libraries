using Enterprise.Api.ErrorHandling.Constants;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Enterprise.Api.Minimal.Services;

public static class EndpointService
{
    public static ProblemHttpResult BadDataShapingRequest(ProblemDetailsFactory problemDetailsFactory, HttpContext httpContext, string? properties)
    {
        IDictionary<string, string[]> errorDictionary = new Dictionary<string, string[]>
        {
            { "properties", ["Not all requested data shaping fields exist on the resource."] }
        };

        ValidationProblemDetails problemDetails =
            problemDetailsFactory.CreateValidationProblemDetails(
                httpContext, errorDictionary, StatusCodes.Status422UnprocessableEntity
            );

        problemDetails.Title = ValidationProblemDetailsConstants.Title;
        problemDetails.Detail = ValidationProblemDetailsConstants.Detail;
        problemDetails.Type = ValidationProblemDetailsConstants.Link;

        return TypedResults.Problem(problemDetails);
    }
}