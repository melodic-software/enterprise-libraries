﻿using Enterprise.Logging.AspNetCore.Options;
using Enterprise.Serilog.AspNetCore.Config;

namespace Enterprise.Api.Logging;

internal class DefaultLoggingMiddlewareConfigOptions : LoggingMiddlewareConfigOptions
{
    internal DefaultLoggingMiddlewareConfigOptions()
    {
        UseProviders = app =>
        {
            app.UseSerilog();
        };
    }
}