using System.Reflection;
using Asp.Versioning;
using Enterprise.Api.Minimal.Assemblies;
using Enterprise.Api.Minimal.EndpointSelection;
using Enterprise.Api.Minimal.Mapping;
using Enterprise.Api.Minimal.Options;
using Enterprise.Options.Core.Services.Singleton;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using static Enterprise.Api.Minimal.Assemblies.AssemblyFallbackService;
using static Enterprise.Api.Minimal.Assemblies.ExplicitAssemblyLogger;

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

        List<Assembly> endpointAssemblies = MinimalApiEndpointAssemblyService.Instance.AssembliesToRegister;

        services.AddEndpoints(endpointAssemblies);
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
        
        List<Assembly> endpointAssemblies = MinimalApiEndpointAssemblyService.Instance.AssembliesToRegister;
        bool explicitAssembliesDefined = endpointAssemblies.Any();

        if (explicitAssembliesDefined)
        {
            LogExplicitAssemblies(endpointAssemblies);
        }
        else
        {
            endpointAssemblies = GetAssemblies();
        }

        // This uses the IMapEndpoints static interface method.
        EndpointMapper.MapEndpoints(app, endpointAssemblies);
        
        // This uses pre-registered services of IMapEndpoint.
        EndpointMappingExtensions.MapEndpoints(app);
    }
}
