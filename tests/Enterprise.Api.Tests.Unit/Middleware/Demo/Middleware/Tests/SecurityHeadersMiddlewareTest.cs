using Enterprise.Testing.AspNetCore.Services;
using Microsoft.AspNetCore.Http;

namespace Enterprise.Api.Tests.Unit.Middleware.Demo.Middleware.Tests;

public class SecurityHeadersMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_ShouldSetExpectedResponseHeaders_WhenInvoked()
    {
        // ARRANGE
        HttpContext httpContext = HttpContextCreationService.CreateDefaultContext();
        static Task Next(HttpContext _) => Task.CompletedTask;

        var middleware = new SecurityHeadersMiddleware(Next);

        // ACT
        await middleware.InvokeAsync(httpContext);

        // ASSERT
        string cspHeader = httpContext.Response.Headers["Content-Security-Policy"].ToString();
        string xContentTypeOptionsHeader = httpContext.Response.Headers["X-Content-Type-Options"].ToString();

        Assert.Equal("default-src 'self';frame-ancestors 'none';", cspHeader);
        Assert.Equal("nosniff", xContentTypeOptionsHeader);
    }
}
