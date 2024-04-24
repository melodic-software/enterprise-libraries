using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Enterprise.Api.Performance;

// TODO: Ensure this is registered with the DI container with a scoped lifetime.

/// <summary>
/// This is an action filter that records and logs the total request duration in milliseconds.
/// Be sure to apply this using the ServiceFilter attribute.
/// https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.servicefilterattribute?view=aspnetcore-8.0
/// To apply this globally see <see cref="LogRequestDurationMiddleware"/>.
/// </summary>
[Obsolete("This is only for demonstration purposes, and eventually will be removed.")]
public class LogRequestDurationFilter : IAsyncActionFilter
{
    private readonly ILogger<LogRequestDurationFilter> _logger;

    public LogRequestDurationFilter(ILogger<LogRequestDurationFilter> logger)
    {
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        Stopwatch stopWatch = Stopwatch.StartNew();

        try
        {
            await next();
        }
        finally
        {
            RequestDurationLoggingService.LogRequestDuration(stopWatch, _logger);
        }
    }
}