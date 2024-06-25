namespace Enterprise.Redis.Options;

public class RedisOptions
{
    public const string ConfigSectionKey = "Custom:Redis";

    public bool EnableRedis { get; set; }
    public string ConnectionStringName { get; set; } = string.Empty;
    public string InstanceName { get; set; } = string.Empty;
}
