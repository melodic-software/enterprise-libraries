using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Enterprise.Api.Performance;

[Obsolete("This is only for demonstration purposes, and eventually will be removed.")]
public class LogRequestDurationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LogRequestDurationMiddleware> _logger;

    public LogRequestDurationMiddleware(RequestDelegate next, ILogger<LogRequestDurationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopWatch = Stopwatch.StartNew();

        try
        {
            await _next(context);
        }
        finally
        {
            RequestDurationLoggingService.LogRequestDuration(stopWatch, _logger);
        }
    }
}
