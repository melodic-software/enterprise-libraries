using Enterprise.Options.Core.Abstract;
using Enterprise.Options.Core.Services.Singleton;
using Enterprise.Options.Monitoring;
using Enterprise.Serialization.Json;
using Enterprise.Serialization.Json.Microsoft;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Enterprise.Options.Registration.Extensions;

public static class RegistrationExtensions
{
    /// <summary>
    /// Registers options of type <typeparamref name="TOptions"/> with the specified configuration.
    /// The options instance will be resolved internally.
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="configSectionKey"></param>
    /// <returns></returns>
    public static TOptions RegisterOptions<TOptions>(this IServiceCollection services, IConfiguration configuration, string? configSectionKey)
        where TOptions : class, new()
    {
        TOptions instance = OptionsInstanceService.Instance.GetOptionsInstance<TOptions>(configuration, configSectionKey);
        services.RegisterOptions(instance, configSectionKey);
        return instance;
    }

    /// <summary>
    /// Registers options of type <typeparamref name="TOptions"/> with the specific instance.
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    /// <param name="services"></param>
    /// <param name="currentValue"></param>
    public static void RegisterOptions<TOptions>(this IServiceCollection services, TOptions currentValue)
        where TOptions : class, new()
    {
        services.RegisterOptions(currentValue, null);
    }

    /// <summary>
    /// Registers options of type <typeparamref name="TOptions"/>.
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    /// <param name="services"></param>
    /// <param name="currentValue"></param>
    /// <param name="configSectionKey"></param>
    public static void RegisterOptions<TOptions>(this IServiceCollection services, TOptions? currentValue, string? configSectionKey)
        where TOptions : class, new()
    {
        RegisterDynamicOptionsMonitor(services, currentValue, configSectionKey);
        RegisterBuiltInDotNetTypes<TOptions>(services);
        RegisterCustomAbstractions<TOptions>(services);
    }

    private static void RegisterDynamicOptionsMonitor<TOptions>(IServiceCollection services, TOptions? currentValue, string? configSectionKey) where TOptions : class, new()
    {
        // Remove any existing registrations, ensuring only one instance is ever registered.
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
    }

    private static void RegisterBuiltInDotNetTypes<TOptions>(IServiceCollection services) where TOptions : class, new()
    {
        // Remove any existing registrations, ensuring only one instance is ever registered.
        services.RemoveAll<IOptionsMonitor<TOptions>>();
        services.RemoveAll<IOptionsSnapshot<TOptions>>();
        services.RemoveAll<IOptions<TOptions>>();

        // Register the DynamicOptionsMonitor<TOptions> instance to serve as
        // IOptions<TOptions>, IOptionsSnapshot<TOptions>, IOptionsMonitor<TOptions>, and IOptionsUpdater<TOptions>.
        // https://learn.microsoft.com/en-us/dotnet/core/extensions/options#options-interfaces
        services.TryAddSingleton<IOptions<TOptions>>(provider => provider.GetRequiredService<DynamicOptionsMonitor<TOptions>>());
        services.TryAddScoped<IOptionsSnapshot<TOptions>>(provider => provider.GetRequiredService<DynamicOptionsMonitor<TOptions>>());
        services.TryAddSingleton<IOptionsMonitor<TOptions>>(provider => provider.GetRequiredService<DynamicOptionsMonitor<TOptions>>());
    }

    private static void RegisterCustomAbstractions<TOptions>(IServiceCollection services) where TOptions : class, new()
    {
        // Remove any existing registrations, ensuring only one instance is ever registered.
        services.RemoveAll<IOptionsUpdater<TOptions>>();
        // This one is our own custom options interface for updating options in memory.
        services.TryAddSingleton<IOptionsUpdater<TOptions>>(provider => provider.GetRequiredService<DynamicOptionsMonitor<TOptions>>());
    }
}
