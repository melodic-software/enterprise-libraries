namespace Enterprise.DI.Core.Registration.Decoration.Delegates;

public delegate TDecorator DecoratorFactory<in TService, out TDecorator>(IServiceProvider provider, TService service)
    where TDecorator : class, TService;

public delegate TService DecoratorFactory<TService>(IServiceProvider provider, TService service);
