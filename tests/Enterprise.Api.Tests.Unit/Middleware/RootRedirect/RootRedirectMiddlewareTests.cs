using Enterprise.Api.Middleware.RootRedirect;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace Enterprise.Api.Tests.Unit.Middleware.RootRedirect;

public class RootRedirectMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_ShouldNotRedirect_WhenSwaggerPrefixIsNull()
    {
        // ARRANGE
        HttpContext? httpContext = Substitute.For<HttpContext>();

        var headers = new HeaderDictionary();
        var authorizationHeaderValue = new StringValues("Bearer ");
        string path = "/";
        var routeValues = new RouteValueDictionary();

        httpContext.Request.Headers.Returns(headers);
        httpContext.Request.Headers.Authorization.Returns(authorizationHeaderValue);
        httpContext.Request.Path.Returns(new PathString(path));
        httpContext.Request.RouteValues.Returns(routeValues);

        bool nextDelegateCalled = false;

        Task Next(HttpContext _)
        {
            nextDelegateCalled = true;
            return Task.CompletedTask;
        }

        ILogger<RootRedirectMiddleware>? logger = Substitute.For<ILogger<RootRedirectMiddleware>>();
        IOptionsMonitor<RootRedirectMiddlewareOptions>? optionsMonitor = Substitute.For<IOptionsMonitor<RootRedirectMiddlewareOptions>>();
        optionsMonitor.CurrentValue.Returns(new RootRedirectMiddlewareOptions
        {
            SwaggerRoutePrefix = null
        });

        var middleware = new RootRedirectMiddleware(Next, logger, optionsMonitor);

        // ACT
        await middleware.InvokeAsync(httpContext);

        // ASSERT
        Assert.True(nextDelegateCalled); // this won't happen if a redirect was made
    }

    [Fact]
    public async Task InvokeAsync_ShouldRedirect_WhenSwaggerPrefixIsProvided()
    {
        // ARRANGE
        const string swaggerRoutePrefix = "swagger";

        HttpContext? httpContext = Substitute.For<HttpContext>();

        var headers = new HeaderDictionary();
        var authorizationHeaderValue = new StringValues();
        string path = "/";
        var routeValues = new RouteValueDictionary();

        httpContext.Request.Headers.Returns(headers);
        httpContext.Request.Headers.Authorization.Returns(authorizationHeaderValue);
        httpContext.Request.Path.Returns(new PathString(path));
        httpContext.Request.RouteValues.Returns(routeValues);

        HttpResponse? response = Substitute.For<HttpResponse>();
        bool wasRedirectedToSwagger = false;

        response.When(x => x.Redirect(Arg.Is<string>(s => s == swaggerRoutePrefix), Arg.Any<bool>()))
            .Do(_ => wasRedirectedToSwagger = true);

        httpContext.Response.Returns(response);

        static Task Next(HttpContext _) => Task.CompletedTask;

        ILogger<RootRedirectMiddleware>? logger = Substitute.For<ILogger<RootRedirectMiddleware>>();
        IOptionsMonitor<RootRedirectMiddlewareOptions>? optionsMonitor = Substitute.For<IOptionsMonitor<RootRedirectMiddlewareOptions>>();
        optionsMonitor.CurrentValue.Returns(new RootRedirectMiddlewareOptions
        {
            SwaggerRoutePrefix = swaggerRoutePrefix
        });

        var middleware = new RootRedirectMiddleware(Next, logger, optionsMonitor);

        // ACT
        await middleware.InvokeAsync(httpContext);

        // ASSERT
        Assert.True(wasRedirectedToSwagger);
    }
}
