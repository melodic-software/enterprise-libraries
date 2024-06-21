﻿using Enterprise.DI.Core.Registration.Abstract;
using Enterprise.Events.Handlers.Dependencies;

namespace Example.Api.Events;

public class EventHandlerRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterEventHandler(provider =>
        {
            return new MyEventHandler();
        });
    }
}