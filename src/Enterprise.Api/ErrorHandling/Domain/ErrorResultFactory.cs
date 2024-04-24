using Enterprise.Api.ErrorHandling.Constants;
using Enterprise.Api.Validation;
using Enterprise.Patterns.ResultPattern.Errors;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Enterprise.Api.ErrorHandling.Domain;

public static class ErrorResultFactory
{
    public static IResult ToResult(IReadOnlyCollection<IError> errors, HttpContext httpContext, ProblemDetailsFactory problemDetailsFactory)
    {
        if (!errors.Any())
            throw new InvalidOperationException("No errors were provided to create an IResult.");

        errors = ErrorDedupeService.DedupeErrors(errors).ToList();

        int statusCode = ErrorStatusCodeMapper.CreateStatusCode(errors, httpContext);

        IReadOnlyCollection<IError> scopedErrors = ErrorFilterService.FilterBy(statusCode, errors);

        IResult result = CreateResult(httpContext, problemDetailsFactory, statusCode, scopedErrors);

        return result;
    }

    private static IResult CreateResult(HttpContext httpContext, ProblemDetailsFactory factory, int statusCode, IReadOnlyCollection<IError> scopedErrors)
    {
        // We can't write anything in the response body for 204.
        if (statusCode == StatusCodes.Status204NoContent)
            return TypedResults.NoContent();

        IEnumerable<IError> meaningfulErrors = scopedErrors.GetMeaningful().ToList();
        IDictionary<string, string[]> errorDictionary = meaningfulErrors.ToDictionary(x => x.Code ?? string.Empty, x => new[] { x.Message });

        ValidationProblemDetails problemDetails = factory.CreateValidationProblemDetails(httpContext, errorDictionary, statusCode);

        if (Activity.Current?.Id != null)
            problemDetails.Extensions[ProblemDetailsConstants.TraceIdExtensionKey] = Activity.Current.Id;

        // TODO: Make this conditional?
        problemDetails.Instance = httpContext.Request.Path;

        return problemDetails.ToValidationProblem();
    }
}