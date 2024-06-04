using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Enterprise.Api.ErrorHandling.Shared;

public static class ValidationProblemService
{
    public static ActionResult CreateValidationProblem(ControllerBase controller)
    {
        IOptions<ApiBehaviorOptions> options = controller.HttpContext.RequestServices
            .GetRequiredService<IOptions<ApiBehaviorOptions>>();

        // Here we have access to the response factory, which can be set in configuration to customize problem details responses.
        ActionContext actionContext = controller.ControllerContext;
        Func<ActionContext, IActionResult> invalidModelStateResponseFactory = options.Value.InvalidModelStateResponseFactory;
        IActionResult actionResult = invalidModelStateResponseFactory(actionContext);

        var result = (ActionResult)actionResult;

        return result;
    }
}
