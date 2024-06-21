namespace Enterprise.DI.Core.Registration.Context.Delegates;

public delegate TService DecoratorFactory<TService>(IServiceProvider provider, TService service);
