using Enterprise.Options.Core.Abstract;
using Enterprise.Options.Core.Singleton;
using Enterprise.Serialization.Json;
using Enterprise.Serialization.Json.Microsoft;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Enterprise.Options.Extensions;

public static class DynamicOptionsMonitorExtensions
{
    public static TOptions RegisterOptions<TOptions>(this IServiceCollection services, IConfiguration configuration, string? configSectionKey) where TOptions : class, new()
    {
        TOptions instance = OptionsInstanceService.Instance.GetOptionsInstance<TOptions>(configuration, configSectionKey);
        services.RegisterOptions(instance, configSectionKey);
        return instance;
    }

    public static void RegisterOptions<TOptions>(this IServiceCollection services, TOptions currentValue) where TOptions : class, new()
    {
        services.RegisterOptions(currentValue, null);
    }

    public static void RegisterOptions<TOptions>(this IServiceCollection services, TOptions? currentValue, string? configSectionKey) where TOptions : class, new()
    {
        // We want to remove any existing registrations and ensure only one instance is ever registered.
        services.RemoveAll<DynamicOptionsMonitor<TOptions>>();

        services.TryAddSingleton(provider =>
        {
            IConfiguration config = provider.GetRequiredService<IConfiguration>();
            ILogger<DynamicOptionsMonitor<TOptions>> logger = provider.GetRequiredService<ILogger<DynamicOptionsMonitor<TOptions>>>();
            ISerializeJson jsonSerializer = new DefaultJsonSerializer();

            IConfigurationSection? configSection = !string.IsNullOrWhiteSpace(configSectionKey)
                ? config.GetSection(configSectionKey)
                : null;

            // TODO: Make this duration configurable?
            // Might just be easier to refer to a thread safe singleton object.
            var debouncePeriod = TimeSpan.FromSeconds(1);

            DynamicOptionsMonitor<TOptions> dynamicOptionsMonitor = new(currentValue, configSection, logger, debouncePeriod, jsonSerializer);

            dynamicOptionsMonitor.OnChange(options =>
            {
                // This singleton service is intended to be used for initial app startup and should not be used after the DI service provider has been built.
                // In case anything does reference it after, we can specify new/updated instances to reduce bugs.
                OptionsInstanceService.Instance.UseInstance(options);
            });

            return dynamicOptionsMonitor;
        });

        // We want to remove any existing registrations and ensure only one instance is ever registered.
        services.RemoveAll<IOptionsMonitor<TOptions>>();
        services.RemoveAll<IOptionsSnapshot<TOptions>>();
        services.RemoveAll<IOptions<TOptions>>();
        services.RemoveAll<IOptionsUpdater<TOptions>>();

        // Register the DynamicOptionsMonitor<TOptions> instance to serve as
        // IOptions<TOptions>, IOptionsSnapshot<TOptions>, IOptionsMonitor<TOptions>, and IOptionsUpdater<TOptions>.
        // https://learn.microsoft.com/en-us/dotnet/core/extensions/options#options-interfaces
        services.TryAddSingleton<IOptions<TOptions>>(provider => provider.GetRequiredService<DynamicOptionsMonitor<TOptions>>());
        services.TryAddScoped<IOptionsSnapshot<TOptions>>(provider => provider.GetRequiredService<DynamicOptionsMonitor<TOptions>>());
        services.TryAddSingleton<IOptionsMonitor<TOptions>>(provider => provider.GetRequiredService<DynamicOptionsMonitor<TOptions>>());

        // This one is our own custom options interface for updating options in memory.
        services.TryAddSingleton<IOptionsUpdater<TOptions>>(provider => provider.GetRequiredService<DynamicOptionsMonitor<TOptions>>());
    }
}
