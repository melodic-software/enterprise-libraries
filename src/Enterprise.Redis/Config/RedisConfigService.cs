using Enterprise.Options.Core.Singleton;
using Enterprise.Redis.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Redis.Config;

public static class RedisConfigService
{
    public static void ConfigureRedis(this IServiceCollection services, IConfiguration configuration)
    {
        RedisConfigOptions configOptions = OptionsInstanceService.Instance.GetOptionsInstance<RedisConfigOptions>(configuration, RedisConfigOptions.ConfigSectionKey);

        if (!configOptions.EnableRedis)
            return;

        string? connectionString = configuration.GetConnectionString(configOptions.RedisConnectionString);

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = connectionString;
            options.InstanceName = configOptions.RedisInstanceName;
        });
    }
}