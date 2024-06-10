using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Enterprise.Cors.Config.Delegates;

public delegate void ConfigureCors(CorsOptions corsOptions, string policyName, string[] allowedOrigins);
