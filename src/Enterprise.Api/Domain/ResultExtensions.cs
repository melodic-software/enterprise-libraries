using Enterprise.Api.ErrorHandling.Domain;
using Enterprise.Patterns.ResultPattern.Model;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Enterprise.Api.Domain;

public static class ResultExtensions
{
    public static ActionResult ToActionResult(this Result result, ControllerBase controller, ProblemDetailsFactory problemDetailsFactory)
    {
        return ErrorActionResultFactory.CreateActionResult(result.Errors, controller, problemDetailsFactory);
    }

    public static ActionResult ToActionResult(this Result result, HttpContext httpContext, ProblemDetailsFactory problemDetailsFactory)
    {
        return ErrorActionResultFactory.CreateActionResult(result.Errors, httpContext, problemDetailsFactory);
    }

    public static IResult ToResult(this Result result, HttpContext httpContext, ProblemDetailsFactory problemDetailsFactory)
    {
        return ErrorResultFactory.ToResult(result.Errors, httpContext, problemDetailsFactory);
    }
}