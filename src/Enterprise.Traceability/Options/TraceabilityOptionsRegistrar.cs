﻿using Enterprise.Options.Core.Abstract;
using Enterprise.Options.Registration.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Traceability.Options;

internal sealed class TraceabilityOptionsRegistrar : IRegisterOptions
{
    public static void RegisterOptions(IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterOptions<OpenTelemetryOptions>(configuration, OpenTelemetryOptions.ConfigKeyName);
    }
}