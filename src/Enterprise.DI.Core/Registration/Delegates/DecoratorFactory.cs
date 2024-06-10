namespace Enterprise.DI.Core.Registration.Delegates;

public delegate TDecorator DecoratorFactory<in TService, out TDecorator>(IServiceProvider provider, TService service) 
    where TDecorator : class, TService;

public delegate TService DecoratorFactory<TService>(IServiceProvider provider, TService service);
