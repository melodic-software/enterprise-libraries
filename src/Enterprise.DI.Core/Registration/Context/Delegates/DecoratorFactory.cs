namespace Enterprise.DI.Core.Registration.Context.Delegates;

public delegate TDecorator DecoratorFactory<in TService, out TDecorator>(IServiceProvider provider, TService service)
    where TDecorator : class, TService;

public delegate TService DecoratorFactory<TService>(IServiceProvider provider, TService service);
