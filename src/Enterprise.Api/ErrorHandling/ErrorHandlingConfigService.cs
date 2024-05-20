using Enterprise.Api.ErrorHandling.ExceptionHandlers;
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
        // https://www.milanjovanovic.tech/blog/global-error-handling-in-aspnetcore-8
        // NOTE: This will not be run if the Hellang middleware is registered.
        //services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddExceptionHandler<ValidationExceptionHandler>();

        services.AddProblemDetails(environment, configuration);
    }
    
    public static void UseErrorHandling(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            // TODO: Do we need to remove this if the custom global error handling middleware is being used?
            app.UseDeveloperExceptionPage();
        }

        // We're relying on the Hellang problem details middleware as our global exception handler.
        // This line can cause a runtime error if problem details service middleware (Microsoft default) is not registered.
        app.UseExceptionHandler();

        app.UseProblemDetails();
    }
}
