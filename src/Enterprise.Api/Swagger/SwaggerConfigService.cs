using Asp.Versioning.ApiExplorer;
using Enterprise.Api.Controllers.Options;
using Enterprise.Api.Swagger.Options;
using Enterprise.Api.Versioning.Options;
using Enterprise.Options.Core.Singleton;
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
        SwaggerConfigOptions swaggerConfigOptions = OptionsInstanceService.Instance
            .GetOptionsInstance<SwaggerConfigOptions>(configuration, SwaggerConfigOptions.ConfigSectionKey);

        if (!swaggerConfigOptions.EnableSwagger)
        {
            return;
        }

        // exposes information on the API, which is used internally by Swashbuckle to create an OpenApi specification
        services.AddEndpointsApiExplorer();

        // This registered service is what configures the Swagger generation options.
        services.AddTransient(RegisterSwaggerGenConfigurer(swaggerConfigOptions));

        // NOTE: The setup action for Swagger generation is not required here.
        // It is handled by the IConfigureOptions<SwaggerGenOptions> registered above.
        services.AddSwaggerGen();

        // I believe this requires a package reference.
        //services.AddSwaggerGenNewtonsoftSupport();
    }

    private static Func<IServiceProvider, IConfigureOptions<SwaggerGenOptions>> RegisterSwaggerGenConfigurer(SwaggerConfigOptions swaggerConfigOptions)
    {
        return serviceProvider =>
        {
            IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
            IOptions<ControllerConfigOptions> controllerConfigOptions = serviceProvider.GetRequiredService<IOptions<ControllerConfigOptions>>();
            IOptions<VersioningConfigOptions> versioningConfigOptions = serviceProvider.GetRequiredService<IOptions<VersioningConfigOptions>>();
            ILogger<SwaggerGenOptionsConfigurer> logger = serviceProvider.GetRequiredService<ILogger<SwaggerGenOptionsConfigurer>>();

            // this is our primary configurer for swagger generation instead of the setupAction that can be passed into services.AddSwaggerGen()
            // we can inject other services in here as needed, which is one advantage over calling .AddSwaggerGen on the IServiceCollection instance
            IConfigureOptions<SwaggerGenOptions> result = new SwaggerGenOptionsConfigurer(
                swaggerConfigOptions,
                controllerConfigOptions,
                versioningConfigOptions,
                configuration,
                logger,
                serviceProvider
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
        SwaggerConfigOptions swaggerConfigOptions = app.Services.GetRequiredService<IOptions<SwaggerConfigOptions>>().Value;

        if (!swaggerConfigOptions.EnableSwagger || app.Environment.IsProduction())
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
            if (swaggerConfigOptions.CanConfigureOAuth)
            {
                options.OAuthClientId(swaggerConfigOptions.OAuthClientId);

                if (!string.IsNullOrWhiteSpace(swaggerConfigOptions.OAuthClientSecret))
                {
                    options.OAuthClientSecret(swaggerConfigOptions.OAuthClientSecret);
                }

                options.OAuthAppName(swaggerConfigOptions.OAuthAppName);

                if (swaggerConfigOptions.UsePkce)
                {
                    options.OAuthUsePkce();
                }

                Dictionary<string, string> queryStringParams = new Dictionary<string, string>();

                if (!string.IsNullOrWhiteSpace(swaggerConfigOptions.OAuthAudience))
                {
                    queryStringParams.Add("audience", swaggerConfigOptions.OAuthAudience);
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
