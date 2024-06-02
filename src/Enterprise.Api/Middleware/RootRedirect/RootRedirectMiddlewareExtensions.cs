using Microsoft.AspNetCore.Builder;

namespace Enterprise.Api.Middleware.RootRedirect;

internal static class RootRedirectMiddlewareExtensions
{
    public static void UseRootRedirectMiddleware(this WebApplication app)
    {
        app.UseMiddleware<RootRedirectMiddleware>();
    }
}
