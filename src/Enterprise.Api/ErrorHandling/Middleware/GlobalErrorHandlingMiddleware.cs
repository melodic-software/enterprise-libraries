using Enterprise.Api.ErrorHandling.Middleware.Constants;
using Enterprise.Api.ErrorHandling.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Enterprise.Api.ErrorHandling.Middleware;
// NOTE: This is an alternate approach to global error handling.
// Take a look at the ConfigureGlobalErrorHandler method in the ErrorHandlingConfigService.

[Obsolete(ObsoleteConstants.UseIExceptionHandlerWarning)]
public class GlobalErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public GlobalErrorHandlingMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // One of the advantages of using this middleware over "app.UseExceptionHandler"
        // is that we can perform pre- and post-handling steps (even if there isn't an error to handle).

        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        await GlobalErrorHandler.HandleErrorAsync(context, exception);
    }
}
