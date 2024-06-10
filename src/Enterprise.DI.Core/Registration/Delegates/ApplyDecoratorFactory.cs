namespace Enterprise.DI.Core.Registration.Delegates;

public delegate ApplyDecorator<TService> ApplyDecoratorFactory<TService>(IServiceProvider provider);
