﻿using Enterprise.ApplicationServices.Core.Queries.Handlers;
using Enterprise.ApplicationServices.Core.Queries.Model;
using Enterprise.ApplicationServices.Decorators.Queries.Handlers;
using Enterprise.ApplicationServices.DI.Queries.Options;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.DI.Core.Registration;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.DI.Queries.Decoration;

public static class DecoratorRegistrationExtensions
{
    public static void WithDefaultDecorators<TQuery, TResponse>(
        this RegistrationContext<IHandleQuery<TQuery, TResponse>> registrationContext)
        where TQuery : IQuery
    {
        registrationContext
            .WithDecorators((provider, queryHandler) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                IEnumerable<IValidator<TQuery>> validators = provider.GetServices<IValidator<TQuery>>();
                IHandleQuery<TQuery, TResponse> decorator = new FluentValidationQueryHandler<TQuery, TResponse>(queryHandler, decoratorService, validators);
                return decorator;
            }, (provider, queryHandler) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                IHandleQuery<TQuery, TResponse> decorator = new NullQueryValidationQueryHandler<TQuery, TResponse>(queryHandler, decoratorService);
                return decorator;
            }, (provider, queryHandler) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                ILogger<ErrorHandlingQueryHandler<TQuery, TResponse>> logger = provider.GetRequiredService<ILogger<ErrorHandlingQueryHandler<TQuery, TResponse>>>();
                IHandleQuery<TQuery, TResponse> decorator = new ErrorHandlingQueryHandler<TQuery, TResponse>(queryHandler, decoratorService, logger);
                return decorator;
            }, (provider, queryHandler) =>
            {
                IGetDecoratedInstance decoratorService = provider.GetRequiredService<IGetDecoratedInstance>();
                ILogger<LoggingQueryHandler<TQuery, TResponse>> logger = provider.GetRequiredService<ILogger<LoggingQueryHandler<TQuery, TResponse>>>();
                IHandleQuery<TQuery, TResponse> decorator = new LoggingQueryHandler<TQuery, TResponse>(queryHandler, decoratorService, logger);
                return decorator;
            });
    }

    internal static void RegisterWithDecorators<TQuery, TResponse>(
        this RegistrationContext<IHandleQuery<TQuery, TResponse>> registration,
        QueryHandlerRegistrationOptions<TQuery, TResponse> options)
        where TQuery : IQuery
    {
        registration.AddQueryHandler(options)
            .WithDefaultDecorators();
    }
}
