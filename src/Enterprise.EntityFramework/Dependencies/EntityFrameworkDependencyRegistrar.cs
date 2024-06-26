﻿using Enterprise.DI.Core.Registration;
using Enterprise.EntityFramework.EntityDeletion;
using Enterprise.EntityFramework.Outbox;
using Enterprise.Patterns.Outbox.Factory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Enterprise.EntityFramework.Dependencies;

internal class EntityFrameworkDependencyRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(provider =>
        {
            ILogger<EntityDeletionInterceptor> logger = provider.GetRequiredService<ILogger<EntityDeletionInterceptor>>();

            return new EntityDeletionInterceptor(logger);
        });

        services.AddScoped(provider =>
        {
            ILogger<OutboxMessagesInterceptor> logger = provider.GetRequiredService<ILogger<OutboxMessagesInterceptor>>();
            OutboxMessageFactory factory = provider.GetRequiredService<OutboxMessageFactory>();

            return new OutboxMessagesInterceptor(logger, factory);
        });
    }
}