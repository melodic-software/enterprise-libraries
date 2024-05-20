using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Enterprise.Api.Performance;

// TODO: Should this be named as an action filter instead of attribute?

/// <summary>
/// This was left here for demonstration purposes.
/// DI is difficult with attributes, requiring something like the service locator anti-pattern.
/// This specific example relies on the HttpContext to get access to the service provider.
/// See <see cref="LogRequestDurationFilter"/> for a better way to apply this in a web API.
/// </summary>
[Obsolete("This is only for demonstration purposes, and eventually will be removed.")]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
internal sealed class LogRequestDurationAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var stopWatch = Stopwatch.StartNew();

        try
        {
            await next();
        }
        finally
        {
            // We have to get the logger this way since constructor injection does not work with attributes.
            // This is a bad approach because it is a service locator (anti-pattern).
            IServiceProvider serviceProvider = context.HttpContext.RequestServices;
            ILogger<LogRequestDurationAttribute> logger = serviceProvider.GetRequiredService<ILogger<LogRequestDurationAttribute>>();

            RequestDurationLoggingService.LogRequestDuration(stopWatch, logger);
        }
    }
}