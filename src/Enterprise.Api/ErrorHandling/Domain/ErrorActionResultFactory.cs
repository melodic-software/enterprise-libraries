using Enterprise.Api.ErrorHandling.Constants;
using Enterprise.Patterns.ResultPattern.Errors;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Enterprise.Api.ErrorHandling.Domain;

public static class ErrorActionResultFactory
{
    public static ActionResult CreateActionResult(
        IReadOnlyCollection<IError> errors,
        ControllerBase controller,
        ProblemDetailsFactory problemDetailsFactory)
    {
        return CreateActionResult(errors, controller.HttpContext, problemDetailsFactory);
    }

    public static ActionResult CreateActionResult(
        IReadOnlyCollection<IError> errors,
        HttpContext httpContext,
        ProblemDetailsFactory problemDetailsFactory)
    {
        if (!errors.Any())
            throw new InvalidOperationException("No errors were provided to create an ActionResult.");

        errors = ErrorDedupeService.DedupeErrors(errors).ToList();

        int statusCode = ErrorStatusCodeService.GetStatusCode(errors, httpContext);

        IEnumerable<IError> scopedErrors = ErrorFilterService.FilterBy(statusCode, errors);

        ActionResult result = CreateActionResult(httpContext, problemDetailsFactory, statusCode, scopedErrors);

        return result;
    }

    private static ActionResult CreateActionResult(HttpContext httpContext, ProblemDetailsFactory factory, int statusCode, IEnumerable<IError> scopedErrors)
    {
        if (statusCode == StatusCodes.Status204NoContent)
            return new NoContentResult();

        IEnumerable<IError> meaningfulErrors = scopedErrors.GetMeaningful().ToList();

        IDictionary<string, string[]> errorDictionary = meaningfulErrors
            .Where(e => e.Code != null)
            .GroupBy(e => e.Code)
            .ToDictionary(g => g.Key!, g => g.Select(e => e.Message).ToArray());

        ValidationProblemDetails problemDetails = factory.CreateValidationProblemDetails(httpContext, errorDictionary, statusCode);

        if (Activity.Current?.Id != null)
            problemDetails.Extensions[ProblemDetailsConstants.TraceIdExtensionKey] = Activity.Current.Id;

        // TODO: Make this conditional?
        problemDetails.Instance = httpContext.Request.Path;

        return new UnprocessableEntityObjectResult(problemDetails);
    }
}
