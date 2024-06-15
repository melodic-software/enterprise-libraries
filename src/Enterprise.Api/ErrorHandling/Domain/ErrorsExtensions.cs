using Enterprise.Patterns.ResultPattern.Errors.Model.Abstract;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Enterprise.Api.ErrorHandling.Domain;

public static class ErrorsExtensions
{
    /// <summary>
    /// Filter out errors that do not have anything meaningful to display or return.
    /// These are errors that have a code or message specified.
    /// </summary>
    /// <param name="errors"></param>
    /// <returns></returns>
    public static IEnumerable<IError> GetMeaningful(this IEnumerable<IError> errors)
    {
        var meaningfulErrors = errors
            .Where(e => !string.IsNullOrWhiteSpace(e.Code) || !string.IsNullOrWhiteSpace(e.Message))
            .ToList();

        return meaningfulErrors;
    }

    public static ActionResult ToActionResult(this IEnumerable<IError> errors, ControllerBase controller, ProblemDetailsFactory problemDetailsFactory)
    {
        return ErrorActionResultFactory.CreateActionResult(errors.ToList(), controller, problemDetailsFactory);
    }

    public static ActionResult ToActionResult(this IEnumerable<IError> errors, HttpContext httpContext, ProblemDetailsFactory problemDetailsFactory)
    {
        return ErrorActionResultFactory.CreateActionResult(errors.ToList(), httpContext, problemDetailsFactory);
    }

    public static IResult ToResult(this IEnumerable<IError> errors, HttpContext httpContext, ProblemDetailsFactory problemDetailsFactory)
    {
        return ErrorResultFactory.ToResult(errors.ToList(), httpContext, problemDetailsFactory);
    }
}
