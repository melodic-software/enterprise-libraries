using Microsoft.AspNetCore.Builder;

namespace Enterprise.Api.Middleware.IgnoreFavicon;

public static class IgnoreFaviconMiddlewareRegistrations
{
    public static void UseIgnoreFaviconMiddleware(this WebApplication app)
    {
        app.UseMiddleware<IgnoreFaviconMiddleware>();
    }
}
