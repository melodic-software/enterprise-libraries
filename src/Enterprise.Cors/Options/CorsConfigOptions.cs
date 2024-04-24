using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Enterprise.Cors.Options;

public class CorsConfigOptions
{
    public const string ConfigSectionKey = "CORS";

    public bool EnableCors { get; set; } = true;
    public HashSet<string> AllowedOrigins { get; set; } = [];
    public Action<CorsOptions>? ConfigureCustom { get; set; } = null;
}