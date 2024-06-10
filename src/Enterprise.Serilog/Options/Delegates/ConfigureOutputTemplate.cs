using Enterprise.Serilog.Templating;
using Microsoft.Extensions.Hosting;

namespace Enterprise.Serilog.Options.Delegates;

public delegate void ConfigureOutputTemplate(IHostApplicationBuilder builder, OutputTemplateBuilder outputTemplateBuilder);
