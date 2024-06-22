namespace Enterprise.DI.Core.Registration.Delegates;

public delegate TService KeyedImplementationFactory<out TService>(IServiceProvider provider, object? serviceKey);
