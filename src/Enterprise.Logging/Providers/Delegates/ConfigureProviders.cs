using Microsoft.Extensions.Hosting;

namespace Enterprise.Logging.Providers.Delegates;

public delegate void ConfigureProviders(IHostApplicationBuilder builder);
