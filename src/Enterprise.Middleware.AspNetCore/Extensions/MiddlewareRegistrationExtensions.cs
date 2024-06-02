using Enterprise.Middleware.AspNetCore.StartupServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Enterprise.Middleware.AspNetCore.Extensions;

public static class MiddlewareRegistrationExtensions
{
    public static void UseListStartupServicesMiddleware(this WebApplication app)
    {
        ILogger<ListStartupServicesMiddleware> logger = app.Services.GetRequiredService<ILogger<ListStartupServicesMiddleware>>();

        app.UseMiddleware<ListStartupServicesMiddleware>(app.Services, logger);
    }
}
