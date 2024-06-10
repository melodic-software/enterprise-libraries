namespace Enterprise.DI.Core.Registration.Context.Delegates;

public delegate ApplyDecorator<TService> ApplyDecoratorFactory<TService>(IServiceProvider provider);
