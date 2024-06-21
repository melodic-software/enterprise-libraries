namespace Enterprise.DI.Registration.Delegates;

public delegate TService ImplementationFactory<out TService>(IServiceProvider provider);
