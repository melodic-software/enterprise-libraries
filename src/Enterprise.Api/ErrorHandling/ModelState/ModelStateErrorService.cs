using Enterprise.Api.Constants;
using Enterprise.Api.ErrorHandling.Constants;
using Enterprise.Constants;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Api.ErrorHandling.ModelState;

public static class ModelStateErrorService
{
    public static IActionResult GetInvalidModelStateResponseFactory(ActionContext context)
    {
        HttpContext httpContext = context.HttpContext;
        PathString requestPath = httpContext.Request.Path;
        IServiceProvider requestServices = httpContext.RequestServices;

        // https://datatracker.ietf.org/doc/html/rfc7807

        // Create a validation problem details object.
        ProblemDetailsFactory problemDetailsFactory = requestServices.GetRequiredService<ProblemDetailsFactory>();

        // This will translate the model data to the RFC format.
        ValidationProblemDetails validationProblemDetails = problemDetailsFactory
            .CreateValidationProblemDetails(httpContext, context.ModelState);

        validationProblemDetails.Title = ValidationProblemDetailsConstants.Title;
        validationProblemDetails.Detail = ValidationProblemDetailsConstants.Detail;
        validationProblemDetails.Status = StatusCodes.Status422UnprocessableEntity;
        validationProblemDetails.Type = StatusCodeLinkConstants.LinkFor(validationProblemDetails.Status.Value);
        validationProblemDetails.Instance = requestPath;

        IActionResult result = new BadRequestObjectResult(validationProblemDetails)
        {
            ContentTypes =
            {
                // Part of the RFC.
                MediaTypeConstants.ProblemPlusJson,
                MediaTypeConstants.ProblemPlusXml
            }
        };

        return result;
    }
}