using Enterprise.Options.Core.Delegates;

namespace Enterprise.Cors.Options;

public class CorsOptions
{
    public const string ConfigSectionKey = "CORS";

    public bool EnableCors { get; set; } = true;
    public HashSet<string> AllowedOrigins { get; set; } = [];
    public Configure<Microsoft.AspNetCore.Cors.Infrastructure.CorsOptions>? CustomConfigure { get; set; }
}
