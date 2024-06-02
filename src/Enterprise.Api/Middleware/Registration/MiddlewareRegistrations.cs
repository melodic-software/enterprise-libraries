using Enterprise.Api.Swagger.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Enterprise.Api.Middleware.Registration;
public static class MiddlewareRegistrations
{
    public static void UseIgnoreFaviconMiddleware(this WebApplication app)
    {
        app.UseMiddleware<IgnoreFaviconMiddleware>();
    }

    public static void UseRootRedirectMiddleware(this WebApplication app)
    {
        ILogger<RootRedirectMiddleware> logger = app.Services.GetRequiredService<ILogger<RootRedirectMiddleware>>();
        string swaggerRoutePrefix = SwaggerConstants.RoutePrefix;

        app.UseMiddleware<RootRedirectMiddleware>(logger, swaggerRoutePrefix);
    }

    public static void UseUserLoggingScopeMiddleware(this WebApplication app)
    {
        app.UseMiddleware<UserLoggingScopeMiddleware>();
    }
}
