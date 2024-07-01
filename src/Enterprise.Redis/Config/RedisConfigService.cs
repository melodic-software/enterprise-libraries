using Enterprise.Exceptions.Model;
using Enterprise.Logging.Core.Loggers;
using Enterprise.Options.Core.Services.Singleton;
using Enterprise.Redis.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Enterprise.Redis.Config;

public static class RedisConfigService
{
    public static void ConfigureRedis(this IServiceCollection services, IHostEnvironment environment, IConfiguration configuration)
    {
        RedisOptions options = OptionsInstanceService.Instance
            .GetOptionsInstance<RedisOptions>(configuration, RedisOptions.ConfigSectionKey);

        if (!options.EnableRedis)
        {
            return;
        }

        string? redisConnectionString = configuration.GetConnectionString(options.ConnectionStringName);

        if (string.IsNullOrWhiteSpace(redisConnectionString))
        {
            throw new ConfigurationException(
                "A Redis connection string is required when Redis has been enabled. " +
                "Please ensure the connection string name is accurate and that a value has been provided."
            );
        }

        try
        {
            // This is a connection to our Redis interface.
            // We register it as a singleton because it will be resolved internally by some services we'll be using.
            // TODO: Cane we use the async version of this?
            IConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
            services.TryAddSingleton(connectionMultiplexer);

            services.AddStackExchangeRedisCache(redisCacheOptions =>
            {
                //options.Configuration = redisConnectionString;
                //options.InstanceName = options.RedisInstanceName;
                redisCacheOptions.ConnectionMultiplexerFactory = () => Task.FromResult(connectionMultiplexer);
            });
        }
        catch (Exception ex)
        {
            // This could happen if we run migrations, in which case a local Redis container likely won't be running.
            PreStartupLogger.Instance.LogError(ex, "Redis could not be registered.");

            if (!environment.IsDevelopment())
            {
                // If this fails in a non development environment, we need to fail fast.
                throw;
            }

            // We don't want to use a fallback unless we're running locally.
            PreStartupLogger.Instance.LogWarning("Falling back to the distributed memory cache.");
            services.AddDistributedMemoryCache();
        }
    }
}
