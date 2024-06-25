using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Enterprise.Cors.Config.Delegates;

public delegate void ConfigureCors(string policyName, CorsOptions corsOptions, Options.CorsOptions customOptions);
