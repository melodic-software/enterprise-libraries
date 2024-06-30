﻿using Enterprise.Api.ErrorHandling.Options;
using Enterprise.Options.Core.Services.Singleton;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Api.ErrorHandling.ExceptionHandlers.Config;
public static class ExceptionHandlerConfigService
{
    public static void RegisterExceptionHandlers(this IServiceCollection services, IConfiguration configuration)
    {
        // TODO: If these are ignored when Hellang middleware is registered, can we just remove this conditional and register them anyway?

        ErrorHandlingOptions options = OptionsInstanceService.Instance
            .GetOptionsInstance<ErrorHandlingOptions>(configuration, ErrorHandlingOptions.ConfigSectionKey);

        if (options.UseHellangMiddleware)
        {
            return;
        }

        // https://www.milanjovanovic.tech/blog/global-error-handling-in-aspnetcore-8
        // NOTE: This will not be run if the Hellang middleware is registered.

        //services.AddExceptionHandler<DefaultExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddExceptionHandler<TimeOutExceptionHandler>();
        services.AddExceptionHandler<ValidationExceptionHandler>();
    }
}
