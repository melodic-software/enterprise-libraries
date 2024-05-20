using Enterprise.Api.ErrorHandling.Constants;
using Enterprise.Api.ErrorHandling.Shared;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Enterprise.Api.Controllers.Extensions;

public static class ApiControllerExtensions
{
    public static IActionResult BadDataShapingRequest(this ControllerBase controller, ProblemDetailsFactory problemDetailsFactory)
    {
        IDictionary<string, string[]> errorDictionary = new Dictionary<string, string[]>
        {
            { "properties", ["Not all requested data shaping fields exist on the resource."] }
        };

        ValidationProblemDetails problemDetails =
            problemDetailsFactory.CreateValidationProblemDetails(
                controller.HttpContext, errorDictionary, StatusCodes.Status422UnprocessableEntity
            );

        problemDetails.Title = ValidationProblemDetailsConstants.Title;
        problemDetails.Detail = ValidationProblemDetailsConstants.Detail;
        problemDetails.Type = ValidationProblemDetailsConstants.Link;

        return controller.BadRequest(problemDetails);
    }

    public static IActionResult? ReValidate<T>(this ControllerBase controller, T model)
    {
        return controller.IsInvalid(model) ? ValidationProblemService.CreateValidationProblem(controller) : null;
    }

    public static bool IsInvalid<T>(this ControllerBase controller, T? model)
    {
        if (Equals(model, default(T)))
        {
            return false;
        }

        string? key = nameof(T);
        ModelStateDictionary modelState = controller.ModelState;

        if (modelState.ContainsKey(key))
        {
            modelState.ClearValidationState(key);
        }
        else
        {
            key = null;
        }

        bool isInvalid = !controller.TryValidateModel(model!, key);

        return isInvalid;
    }
}
