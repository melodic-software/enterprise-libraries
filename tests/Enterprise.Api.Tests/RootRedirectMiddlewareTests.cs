using Enterprise.Api.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Moq;
using Xunit;

namespace Enterprise.Api.Tests;

public class RootRedirectMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_Invoke_MustNotRedirectWhenSwaggerPrefixIsNull()
    {
        // ARRANGE
        Mock<HttpContext> httpContextMock = new();

        IHeaderDictionary headers = new HeaderDictionary();
        StringValues authorizationHeaderValue = new StringValues("Bearer ");
        string path = "/";
        RouteValueDictionary routeValues = new RouteValueDictionary(); //  RouteValueDictionary.FromArray()

        httpContextMock.Setup(x => x.Request.Headers).Returns(headers);
        httpContextMock.Setup(x => x.Request.Headers.Authorization).Returns(authorizationHeaderValue);
        httpContextMock.Setup(x => x.Request.Path).Returns(path);
        httpContextMock.Setup(x => x.Request.RouteValues).Returns(routeValues);

        bool nextDelegateCalled = false;

        RequestDelegate next = _ =>
        {
            nextDelegateCalled = true;
            return Task.CompletedTask;
        };

        Mock<ILogger<RootRedirectMiddleware>> loggerMock = new();
        const string? swaggerRoutePrefix = null;

        RootRedirectMiddleware middleware = new RootRedirectMiddleware(next, loggerMock.Object, swaggerRoutePrefix);

        // ACT
        await middleware.InvokeAsync(httpContextMock.Object);

        // ASSERT
        Assert.True(nextDelegateCalled); // this won't happen if a redirect was made
    }

    [Fact]
    public async Task InvokeAsync_Invoke_MustRedirect()
    {
        // ARRANGE
        const string? swaggerRoutePrefix = "swagger";

        Mock<HttpContext> httpContextMock = new();

        IHeaderDictionary headers = new HeaderDictionary();
        StringValues authorizationHeaderValue = [];
        string path = "/";
        RouteValueDictionary routeValues = new RouteValueDictionary(); //  RouteValueDictionary.FromArray()

        httpContextMock.Setup(x => x.Request.Headers).Returns(headers);
        httpContextMock.Setup(x => x.Request.Headers.Authorization).Returns(authorizationHeaderValue);
        httpContextMock.Setup(x => x.Request.Path).Returns(path);
        httpContextMock.Setup(x => x.Request.RouteValues).Returns(routeValues);

        Mock<HttpResponse> responseMock = new();
        bool wasRedirectedToSwagger = false;

        responseMock.Setup(x =>
                x.Redirect(It.Is<string>(s => s == swaggerRoutePrefix), It.IsAny<bool>()))
            .Callback(() => wasRedirectedToSwagger = true);

        httpContextMock.Setup(x => x.Response).Returns(responseMock.Object);

        RequestDelegate next = _ => Task.CompletedTask;

        Mock<ILogger<RootRedirectMiddleware>> loggerMock = new();
           
        RootRedirectMiddleware middleware = new RootRedirectMiddleware(next, loggerMock.Object, swaggerRoutePrefix);

        // ACT
        await middleware.InvokeAsync(httpContextMock.Object);

        // ASSERT
        Assert.True(wasRedirectedToSwagger);
    }
}