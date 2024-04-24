using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Api.ErrorHandling.ProblemDetailsMiddleware;

public static class ProblemDetailsConfigService
{
    internal static void AddProblemDetails(this IServiceCollection services, IWebHostEnvironment environment, IConfiguration configuration)
    {
        // The default out of the box Microsoft "ProblemDetails" registrations.
        services.AddProblemDetails();

        // This configures the problem details using the "Hellang" middleware.
        HellangMiddlewareService.AddProblemDetails(services, environment, configuration);
    }

    internal static void UseProblemDetails(this WebApplication app)
    {
        HellangMiddlewareService.UseProblemDetails(app);
    }
}