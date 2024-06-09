using System.Reflection;
using Asp.Versioning;
using Enterprise.Api.Minimal.EndpointSelection;
using Enterprise.Api.Minimal.Mapping;
using Enterprise.Api.Minimal.Options;
using Enterprise.Logging.Core.Loggers;
using Enterprise.Options.Core.Services.Singleton;
using Enterprise.Reflection.Assemblies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using static Enterprise.Reflection.Assemblies.Delegates.AssemblyNameFilters;

namespace Enterprise.Api.Minimal;

internal static class EndpointConfigService
{
    internal static void RegisterMinimalApiEndpointSelectorPolicy(this IServiceCollection services, IConfiguration configuration)
    {
        MinimalApiOptions options = OptionsInstanceService.Instance
            .GetOptionsInstance<MinimalApiOptions>(configuration, MinimalApiOptions.ConfigSectionKey);

        if (!options.EnableMinimalApiEndpoints)
        {
            return;
        }

        // TODO: Is this the right service scope and type param for <T> here?
        services.AddSingleton(provider =>
        {
            ILogger<PreferMinimalApiEndpointSelectorPolicy> logger = provider.GetRequiredService<ILogger<PreferMinimalApiEndpointSelectorPolicy>>();
            IOptions<ApiVersioningOptions>? apiVersioningOptions = provider.GetService<IOptions<ApiVersioningOptions>>();

            MatcherPolicy matcherPolicy = new PreferMinimalApiEndpointSelectorPolicy(logger, apiVersioningOptions);

            return matcherPolicy;
        });

        services.AddEndpoints(options.EndpointAssemblies);
    }

    internal static void MapEndpoints(this WebApplication app)
    {
        IOptions<MinimalApiOptions> options = app.Services.GetRequiredService<IOptions<MinimalApiOptions>>();
        app.MapEndpoints(options.Value);
    }

    internal static void MapEndpoints(this WebApplication app, MinimalApiOptions options)
    {
        if (!options.EnableMinimalApiEndpoints)
        {
            app.Logger.LogInformation("Minimal API endpoints have been disabled.");
            return;
        }
        
        List<Assembly> endpointAssemblies = options.EndpointAssemblies;
        bool explicitAssembliesDefined = endpointAssemblies.Any();

        if (!explicitAssembliesDefined)
        {
            PreStartupLogger.Instance.LogInformation(
                "Explicit assemblies containing minimal API endpoints have not been specified. " +
                "Loading solution assemblies."
            );
            
            endpointAssemblies = AssemblyLoader
                .LoadSolutionAssemblies(ThatAreNotMicrosoft)
                .ToList();
        }

        if (explicitAssembliesDefined)
        {
            PreStartupLogger.Instance.LogInformation("Registering minimal API endpoints for the explicitly defined assemblies.");

            foreach (Assembly assembly in endpointAssemblies)
            {
                PreStartupLogger.Instance.LogInformation("{AssemblyName}", assembly.FullName);
            }
        }

        // This uses the IMapEndpoints static interface method.
        EndpointMapper.MapEndpoints(app, endpointAssemblies);
        
        // This uses pre-registered services of IMapEndpoint
        EndpointMappingExtensions.MapEndpoints(app, null);
    }
}
