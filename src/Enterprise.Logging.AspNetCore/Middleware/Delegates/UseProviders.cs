using Microsoft.AspNetCore.Builder;

namespace Enterprise.Logging.AspNetCore.Middleware.Delegates;

public delegate void UseProviders(WebApplication app);
