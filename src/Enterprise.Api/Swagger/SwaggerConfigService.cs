using Asp.Versioning.ApiExplorer;
using Enterprise.Api.Controllers.Options;
using Enterprise.Api.Swagger.Options;
using Enterprise.Api.Versioning.Options;
using Enterprise.Options.Core.Services.Singleton;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.Hosting;
using static Enterprise.Api.Swagger.Constants.SwaggerConstants;
using static Enterprise.Api.Swagger.Endpoints.SwaggerEndpointService;
using static Enterprise.Api.Swagger.UI.SwaggerUICustomizer;

namespace Enterprise.Api.Swagger;
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
        services.AddTransient(RegisterSwaggerGenConfigurer(options, services));

        // NOTE: The setup action for Swagger generation is not required here.
        // It is handled by the IConfigureOptions<SwaggerGenOptions> registered above.
        services.AddSwaggerGen();

        // I believe this requires a package reference.
        //services.AddSwaggerGenNewtonsoftSupport();
    }

    private static Func<IServiceProvider, IConfigureOptions<SwaggerGenOptions>> RegisterSwaggerGenConfigurer(SwaggerOptions options, IServiceCollection services)
    {
        return serviceProvider =>
        {
            IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
            IOptions<ControllerOptions> controllerOptions = serviceProvider.GetRequiredService<IOptions<ControllerOptions>>();
            IOptions<VersioningOptions> versioningOptions = serviceProvider.GetRequiredService<IOptions<VersioningOptions>>();
            ILogger<SwaggerGenOptionsConfigurer> logger = serviceProvider.GetRequiredService<ILogger<SwaggerGenOptionsConfigurer>>();

            // this is our primary configurer for swagger generation instead of the setupAction that can be passed into services.AddSwaggerGen()
            // we can inject other services in here as needed, which is one advantage over calling .AddSwaggerGen on the IServiceCollection instance
            IConfigureOptions<SwaggerGenOptions> result = new SwaggerGenOptionsConfigurer(
                options,
                controllerOptions,
                versioningOptions,
                configuration,
                logger,
                serviceProvider,
                services
            );

            return result;
        };
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

        // Add the middleware that uses the spec to generate the Swagger UI.
        app.UseSwaggerUI(options =>
        {
            if (swaggerOptions.CanConfigureOAuth)
            {
                options.OAuthClientId(swaggerOptions.OAuthClientId);

                if (!string.IsNullOrWhiteSpace(swaggerOptions.OAuthClientSecret))
                {
                    options.OAuthClientSecret(swaggerOptions.OAuthClientSecret);
                }

                options.OAuthAppName(swaggerOptions.OAuthAppName);

                if (swaggerOptions.UsePkce)
                {
                    options.OAuthUsePkce();
                }

                var queryStringParams = new Dictionary<string, string>();

                if (!string.IsNullOrWhiteSpace(swaggerOptions.OAuthAudience))
                {
                    queryStringParams.Add("audience", swaggerOptions.OAuthAudience);
                }

                if (queryStringParams.Any())
                {
                    options.OAuthAdditionalQueryStringParams(queryStringParams);
                }
            }

            IApiVersionDescriptionProvider? descriptionProvider = app.Services.GetService<IApiVersionDescriptionProvider>();

            ConfigureSwaggerEndpoints(options, descriptionProvider);

            options.RoutePrefix = RoutePrefix;

            CustomizeUI(options, app.Configuration);
        });

        // this was causing invalid resource URIs to return a 200 OK instead of a 404
        //app.MapFallback(() => TypedResults.Redirect("/swagger"));
    }
}
