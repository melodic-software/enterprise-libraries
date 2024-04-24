using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Enterprise.Serilog.AspNetCore.Dependencies;

public static class SerilogRegistrar
{
    public static void RegisterSingletonLogger(this WebApplicationBuilder builder, ILogger logger)
    {
        builder.Services.AddSingleton(logger);
    }
}