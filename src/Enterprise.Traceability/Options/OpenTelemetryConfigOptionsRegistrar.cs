﻿using Enterprise.Options.Core.Abstract;
using Enterprise.Options.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Traceability.Options;

public class OpenTelemetryConfigOptionsRegistrar : IRegisterOptions
{
    public static void RegisterOptions(IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterOptions<OpenTelemetryConfigOptions>(configuration, OpenTelemetryConfigOptions.ConfigKeyName);
    }
}