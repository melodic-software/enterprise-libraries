namespace Enterprise.Cors.Options;

public class CorsOptions
{
    public const string ConfigSectionKey = "CORS";

    public bool EnableCors { get; set; } = true;
    public HashSet<string> AllowedOrigins { get; set; } = [];
    public Action<Microsoft.AspNetCore.Cors.Infrastructure.CorsOptions>? CustomConfigure { get; set; }
}
