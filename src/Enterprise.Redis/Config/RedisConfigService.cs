using Enterprise.Exceptions;
using Enterprise.Options.Core.Singleton;
using Enterprise.Redis.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StackExchange.Redis;

namespace Enterprise.Redis.Config;

public static class RedisConfigService
{
    public static void ConfigureRedis(this IServiceCollection services, IConfiguration configuration)
    {
        RedisConfigOptions configOptions = OptionsInstanceService.Instance
            .GetOptionsInstance<RedisConfigOptions>(configuration, RedisConfigOptions.ConfigSectionKey);

        if (!configOptions.EnableRedis)
            return;

        string? redisConnectionString = configuration.GetConnectionString(configOptions.RedisConnectionStringName);

        if (string.IsNullOrWhiteSpace(redisConnectionString))
        {
            throw new ConfigurationException(
                "A Redis connection string is required when Redis has been enabled. " +
                "Please ensure the connection string name is accurate and that a value has been provided."
            );
        }
            
        // This is a connection to our Redis interface.
        // We register it as a singleton because it will be resolved internally by some services we'll be using.
        // TODO: use async version of this?
        IConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
        services.TryAddSingleton(connectionMultiplexer);

        services.AddStackExchangeRedisCache(options =>
        {
            //options.Configuration = redisConnectionString;
            //options.InstanceName = configOptions.RedisInstanceName;
            options.ConnectionMultiplexerFactory = () => Task.FromResult(connectionMultiplexer);
        });
    }
}
