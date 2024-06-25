using AspNetCore = Microsoft.AspNetCore.Cors.Infrastructure;

namespace Enterprise.Cors.Options;

public class CorsOptions
{
    public const string ConfigSectionKey = "CORS";

    public bool EnableCors { get; set; } = true;
    public bool AllowCredentials { get; set; }
    public HashSet<string> AllowedOrigins { get; set; } = [];
    public HashSet<string> AllowedMethods { get; set; } = [];
    public HashSet<string> AllowedHeaders { get; set; } = [];
    public HashSet<string> ExposedHeaders { get; set; } = [];
    public Action<AspNetCore.CorsOptions>? CustomConfigure { get; set; }
}
