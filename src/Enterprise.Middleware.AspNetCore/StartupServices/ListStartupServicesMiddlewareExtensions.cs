using Microsoft.AspNetCore.Builder;

namespace Enterprise.Middleware.AspNetCore.StartupServices;

public static class ListStartupServicesMiddlewareExtensions
{
    public static void UseListStartupServicesMiddleware(this WebApplication app)
    {
        app.UseMiddleware<ListStartupServicesMiddleware>();
    }
}
