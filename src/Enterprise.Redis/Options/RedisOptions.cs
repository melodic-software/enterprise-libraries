﻿namespace Enterprise.Redis.Options;

public class RedisOptions
{
    public const string ConfigSectionKey = "Custom:Redis";

    public bool EnableRedis { get; set; }
    public string RedisConnectionStringName { get; set; } = string.Empty;
    public string RedisInstanceName { get; set; } = string.Empty;
}