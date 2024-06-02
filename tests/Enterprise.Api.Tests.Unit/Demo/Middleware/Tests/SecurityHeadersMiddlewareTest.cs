using Enterprise.Testing.AspNetCore.Services;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Enterprise.Api.Tests.Unit.Demo.Middleware.Tests;

public class SecurityHeadersMiddlewareTest
{
    [Fact]
    public async Task InvokeAsync_Invoke_SetsExpectedResponseHeaders()
    {
        // ARRANGE
        HttpContext httpContext = HttpContextCreationService.CreateDefaultContext();
        RequestDelegate next = _ => Task.CompletedTask;

        var middleware = new SecurityHeadersMiddleware();

        // ACT
        await middleware.InvokeAsync(httpContext, next);

        // ASSERT
        string cspHeader = httpContext.Request.Headers["Content-Security-Policy"].ToString();
        string xContentTypeOptionsHeader = httpContext.Request.Headers["X-Content-Type-Options"].ToString();

        Assert.Equal("default-src 'self';frame-ancestors 'none';", cspHeader);
        Assert.Equal("nosniff", xContentTypeOptionsHeader);
    }
}
