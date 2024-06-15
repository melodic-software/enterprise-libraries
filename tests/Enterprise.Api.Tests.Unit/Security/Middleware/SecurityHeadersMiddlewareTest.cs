using Enterprise.Api.Security.Middleware;
using Microsoft.AspNetCore.Http;

namespace Enterprise.Api.Tests.Unit.Security.Middleware;

public class SecurityHeadersMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_ShouldSetExpectedResponseHeaders_WhenInvoked()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        static Task Next(HttpContext _) => Task.CompletedTask;

        var middleware = new SecurityHeadersMiddleware(Next);

        // Act
        await middleware.InvokeAsync(httpContext);

        // Assert
        string cspHeader = httpContext.Response.Headers["Content-Security-Policy"].ToString();
        string xContentTypeOptionsHeader = httpContext.Response.Headers["X-Content-Type-Options"].ToString();

        Assert.Equal("default-src 'self';frame-ancestors 'none';", cspHeader);
        Assert.Equal("nosniff", xContentTypeOptionsHeader);
    }

    [Fact]
    public async Task InvokeAsync_ShouldNotSetHeaders_WhenResponseHasStarted()
    {
        // Arrange
        HttpContext? httpContext = Substitute.For<HttpContext>();
        HttpResponse? response = Substitute.For<HttpResponse>();
        response.HasStarted.Returns(true);
        httpContext.Response.Returns(response);

        static Task Next(HttpContext _) => Task.CompletedTask;
        var middleware = new SecurityHeadersMiddleware(Next);

        // Act
        await middleware.InvokeAsync(httpContext);

        // Assert
        Assert.False(httpContext.Response.Headers.ContainsKey("Content-Security-Policy"));
        Assert.False(httpContext.Response.Headers.ContainsKey("X-Content-Type-Options"));
    }

    [Fact]
    public async Task InvokeAsync_ShouldCallNextDelegate_WhenInvoked()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        bool nextDelegateCalled = false;

        Task Next(HttpContext _)
        {
            nextDelegateCalled = true;
            return Task.CompletedTask;
        }

        var middleware = new SecurityHeadersMiddleware(Next);

        // Act
        await middleware.InvokeAsync(httpContext);

        // Assert
        Assert.True(nextDelegateCalled);
    }
}
