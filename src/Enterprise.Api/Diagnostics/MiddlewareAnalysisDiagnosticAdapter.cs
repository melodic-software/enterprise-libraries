using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DiagnosticAdapter;
using Microsoft.Extensions.Logging;
using static Enterprise.Api.Diagnostics.MiddlewareAnalysisDiagnosticNames;

namespace Enterprise.Api.Diagnostics;

public class MiddlewareAnalysisDiagnosticAdapter
{
    private readonly ILogger<MiddlewareAnalysisDiagnosticAdapter> _logger;
    public MiddlewareAnalysisDiagnosticAdapter(ILogger<MiddlewareAnalysisDiagnosticAdapter> logger)
    {
        _logger = logger;
    }

    [DiagnosticName(MiddlewareStarting)]
    public void OnMiddlewareStarting(HttpContext httpContext, string name)
    {
        _logger.LogDebug(
            "MiddlewareStarting: '{MiddlewareName}'; " +
            "Request Path: '{RequestPath}'",
            name, httpContext.Request.Path
        );
    }

    [DiagnosticName(MiddlewareException)]
    public void OnMiddlewareException(Exception exception, string name)
    {
        _logger.LogDebug(exception: exception,
            "MiddlewareException: '{MiddlewareName}'; 'Exception: '{Exception}'",
            name, exception
        );
    }

    [DiagnosticName(MiddlewareFinished)]
    public void OnMiddlewareFinished(HttpContext httpContext, string name)
    {
        _logger.LogDebug(
            "MiddlewareFinished: '{MiddlewareName}'; " +
            "Status: '{ResponseStatusCode}'",
            name, httpContext.Response.StatusCode
        );
    }
}
