﻿using Enterprise.DI.Core.Registration.Abstract;
using Enterprise.EntityFramework.EntityDeletion;
using Enterprise.EntityFramework.Outbox;
using Enterprise.Patterns.Outbox.Factory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Enterprise.EntityFramework.Interceptors;

internal sealed class ServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.TryAddScoped(provider =>
        {
            ILogger<EntityDeletionInterceptor> logger = provider.GetRequiredService<ILogger<EntityDeletionInterceptor>>();

            return new EntityDeletionInterceptor(logger);
        });

        services.TryAddScoped(provider =>
        {
            ILogger<OutboxMessagesInterceptor> logger = provider.GetRequiredService<ILogger<OutboxMessagesInterceptor>>();
            EventOutboxMessageFactory factory = provider.GetRequiredService<EventOutboxMessageFactory>();

            return new OutboxMessagesInterceptor(logger, factory);
        });
    }
}
