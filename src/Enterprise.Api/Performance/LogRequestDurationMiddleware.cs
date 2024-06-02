using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Enterprise.Api.Performance;

[Obsolete("This is only for demonstration purposes, and eventually will be removed.")]
public class LogRequestDurationMiddleware : IMiddleware
{
    private readonly ILogger<LogRequestDurationMiddleware> _logger;

    public LogRequestDurationMiddleware(ILogger<LogRequestDurationMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var stopWatch = Stopwatch.StartNew();

        try
        {
            await next(context);
        }
        finally
        {
            RequestDurationLoggingService.LogRequestDuration(stopWatch, _logger);
        }
    }
}
