using Microsoft.Extensions.Hosting;
using Serilog;

namespace Enterprise.Serilog.Options.Delegates;

public delegate void ConfigureLogger(HostBuilderContext context, LoggerConfiguration config);
