using Microsoft.AspNetCore.Http;

namespace Enterprise.Api.Security.Middleware;

/// <summary>
/// Middleware to add security headers to the HTTP response.
/// </summary>
public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// Initializes a new instance of the <see cref="SecurityHeadersMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    public SecurityHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Invokes the middleware to add security headers to the HTTP response.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns>A task that represents the completion of request processing.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        // Ensure headers are only set if the response has not started.
        if (!context.Response.HasStarted)
        {
            IHeaderDictionary headers = context.Response.Headers;

            // Add Content-Security-Policy (CSP) header to prevent XSS attacks and other
            // code injection attacks by specifying what sources are allowed.
            headers["Content-Security-Policy"] = "default-src 'self';frame-ancestors 'none';";

            // Add X-Content-Type-Options header to prevent MIME types from being interpreted incorrectly.
            headers["X-Content-Type-Options"] = "nosniff";
        }

        // Call the next middleware in the pipeline.
        await _next(context);
    }
}
