using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Enterprise.Api.Controllers.ActionFilters;

/// <summary>
/// Action filter that restricts the action's availability to the development environment.
/// </summary>
public sealed class DevOnlyActionFilterAttribute : ActionFilterAttribute
{
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<DevOnlyActionFilterAttribute> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DevOnlyActionFilterAttribute"/> class.
    /// </summary>
    /// <param name="environment">Provides information about the web hosting environment an application is running in.</param>
    /// <param name="logger">Represents a type used to perform logging.</param>
    public DevOnlyActionFilterAttribute(IWebHostEnvironment environment, ILogger<DevOnlyActionFilterAttribute> logger)
    {
        _logger = logger;
        _environment = environment;
    }

    /// <summary>
    /// Called before the action method executes.
    /// </summary>
    /// <param name="context">The action executing context.</param>
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!_environment.IsDevelopment())
        {
            _logger.LogWarning("This action is only enabled in the development environment.");
            context.Result = new NotFoundResult();
        }
        else
        {
            base.OnActionExecuting(context);
        }
    }
}
