using Enterprise.Api.Swagger.Options;
using Enterprise.Options.Core.Services.Singleton;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using static Enterprise.Api.Swagger.Constants.SwaggerConstants;
using static Enterprise.Api.Swagger.SwaggerGen.SwaggerGenConfigurerRegistrar;
using static Enterprise.Api.Swagger.UI.SwaggerUIConfigurer;

namespace Enterprise.Api.Swagger.Config;
// https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle

public static class SwaggerConfigService
{
    public static void ConfigureSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        SwaggerOptions options = OptionsInstanceService.Instance
            .GetOptionsInstance<SwaggerOptions>(configuration, SwaggerOptions.ConfigSectionKey);

        if (!options.EnableSwagger)
        {
            return;
        }

        // exposes information on the API, which is used internally by Swashbuckle to create an OpenApi specification
        services.AddEndpointsApiExplorer();

        // This registered service is what configures the Swagger generation options.
        services.AddTransient(RegisterSwaggerGenConfigurer(services));

        // NOTE: The setup action for Swagger generation is not required here.
        // It is handled by the IConfigureOptions<SwaggerGenOptions> registered above.
        services.AddSwaggerGen();

        // I believe this requires a package reference.
        //services.AddSwaggerGenNewtonsoftSupport();
    }



    /// <summary>
    /// This will add the middleware to the pipeline that generates the OpenAPI specification,
    /// and the middleware that uses that spec to generate the Swagger UI.
    /// Swagger can conditionally be enabled/disabled via configuration, but can never be enabled in production (for security reasons).
    /// </summary>
    /// <param name="app"></param>
    public static void UseSwagger(this WebApplication app)
    {
        SwaggerOptions swaggerOptions = app.Services.GetRequiredService<IOptions<SwaggerOptions>>().Value;

        if (!swaggerOptions.EnableSwagger || app.Environment.IsProduction())
        {
            return;
        }

        // Add the middleware that generates the OpenAPI specification.
        app.UseSwagger(options =>
        {
            options.RouteTemplate = RouteTemplate;
        });

        SwaggerSecurityOptions swaggerSecurityOptions = app.Services.GetRequiredService<IOptions<SwaggerSecurityOptions>>().Value;
        SwaggerUIOptions swaggerUIOptions = app.Services.GetRequiredService<IOptions<SwaggerUIOptions>>().Value;

        // Add the middleware that uses the spec to generate the Swagger UI.
        app.UseSwaggerUI(options => Configure(app, swaggerSecurityOptions, swaggerUIOptions, options));

        // This was causing invalid resource URIs to return a 200 OK instead of a 404.
        //app.MapFallback(() => TypedResults.Redirect("/swagger"));
    }
}
