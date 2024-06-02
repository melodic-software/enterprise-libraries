using Microsoft.AspNetCore.Builder;

namespace Enterprise.Api.Middleware.UserLoggingScope;

public static class UserLoggingScopeMiddlewareExtensions
{
    public static void UseUserLoggingScopeMiddleware(this WebApplication app)
    {
        app.UseMiddleware<UserLoggingScopeMiddleware>();
    }
}
