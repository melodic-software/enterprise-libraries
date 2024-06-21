namespace Enterprise.DI.Registration.Context.Delegates;

public delegate TService DecoratorFactory<TService>(IServiceProvider provider, TService service);
