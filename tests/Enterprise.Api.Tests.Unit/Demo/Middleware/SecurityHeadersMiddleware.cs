using Microsoft.AspNetCore.Http;

namespace Enterprise.Api.Tests.Unit.Demo.Middleware;

public class SecurityHeadersMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        IHeaderDictionary headers = context.Request.Headers;

        // add CSP + X-Content-Type
        headers["Content-Security-Policy"] = "default-src 'self';frame-ancestors 'none';";
        headers["X-Content-Type-Options"] = "nosniff";

        await next(context);
    }
}
