using Asp.Versioning;
using Enterprise.Api.Minimal.EndpointSelection;
using Enterprise.Api.Minimal.Mapping;
using Enterprise.Api.Minimal.Options;
using Enterprise.Options.Core.Singleton;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Enterprise.Api.Minimal;

internal static class EndpointConfigService
{
    internal static void RegisterMinimalApiEndpointSelectorPolicy(this IServiceCollection services, IConfiguration configuration)
    {
        MinimalApiConfigOptions minimalApiConfigOptions = OptionsInstanceService.Instance
            .GetOptionsInstance<MinimalApiConfigOptions>(configuration, MinimalApiConfigOptions.ConfigSectionKey);

        if (!minimalApiConfigOptions.EnableMinimalApiEndpoints)
            return;

        services.AddSingleton(provider =>
        {
            ILogger<PreferMinimalApiEndpointSelectorPolicy> logger = provider.GetRequiredService<ILogger<PreferMinimalApiEndpointSelectorPolicy>>();
            IOptions<ApiVersioningOptions>? apiVersioningOptions = provider.GetService<IOptions<ApiVersioningOptions>>();

            MatcherPolicy matcherPolicy = new PreferMinimalApiEndpointSelectorPolicy(logger, apiVersioningOptions);

            return matcherPolicy;
        });
    }

    internal static void MapEndpoints(this WebApplication app)
    {
        IOptions<MinimalApiConfigOptions> options = app.Services.GetRequiredService<IOptions<MinimalApiConfigOptions>>();
        app.MapEndpoints(options.Value);
    }

    internal static void MapEndpoints(this WebApplication app, MinimalApiConfigOptions configOptions)
    {
        if (!configOptions.EnableMinimalApiEndpoints)
        {
            app.Logger.LogInformation("Minimal API endpoints have been disabled.");
            return;
        }

        // TODO: Provide configuration about HOW this happens.
        // TODO: Create options object for minimal API endpoints.
        // Automatic resolution, list of assemblies, OR one off registrations.
        EndpointMapper.MapEndpoints(app);

        // TODO: Figure out how best to use this with route group builders.
        app.MapEndpoints();
    }
}
