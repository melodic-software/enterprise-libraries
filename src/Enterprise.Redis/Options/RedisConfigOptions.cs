namespace Enterprise.Redis.Options;

public class RedisConfigOptions
{
    public const string ConfigSectionKey = "Custom:Redis";

    public bool EnableRedis { get; set; } = false;
    public string RedisConnectionStringName { get; set; } = string.Empty;
    public string RedisInstanceName { get; set; } = string.Empty;
}
