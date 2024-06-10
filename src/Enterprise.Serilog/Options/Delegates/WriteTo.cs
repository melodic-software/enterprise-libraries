using Microsoft.Extensions.Hosting;
using Serilog;

namespace Enterprise.Serilog.Options.Delegates;

public delegate void WriteTo(IHostApplicationBuilder builder, LoggerConfiguration config, string outputTemplate);
