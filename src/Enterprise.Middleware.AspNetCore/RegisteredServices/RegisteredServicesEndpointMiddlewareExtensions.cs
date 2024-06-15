using Microsoft.AspNetCore.Builder;

namespace Enterprise.Middleware.AspNetCore.RegisteredServices;

public static class RegisteredServicesEndpointMiddlewareExtensions
{
    public static void UseRegisteredServicesEndpointMiddleware(this WebApplication app)
    {
        app.UseMiddleware<RegisteredServicesEndpointMiddleware>();
    }
}
