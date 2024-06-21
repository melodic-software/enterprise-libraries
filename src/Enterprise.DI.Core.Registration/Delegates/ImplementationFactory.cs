namespace Enterprise.DI.Core.Registration.Delegates;

public delegate TService ImplementationFactory<out TService>(IServiceProvider provider);
