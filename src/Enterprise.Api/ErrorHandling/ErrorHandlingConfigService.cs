using Enterprise.Api.ErrorHandling.ExceptionHandlers.Config;
using Enterprise.Api.ErrorHandling.ProblemDetailsMiddleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Enterprise.Api.ErrorHandling;

public static class ErrorHandlingConfigService
{
    public static void ConfigureErrorHandling(this IServiceCollection services, IWebHostEnvironment environment, IConfiguration configuration)
    {
        services.RegisterExceptionHandlers(configuration);
        services.AddProblemDetails(environment, configuration);
    }
    
    public static void UseErrorHandling(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            // TODO: Do we need to remove this if the custom global error handling middleware is being used?
            app.UseDeveloperExceptionPage();
        }

        app.UseExceptionHandler();
        app.UseProblemDetails();
    }
}
