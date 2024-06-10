namespace Enterprise.DI.Core.Registration.Decoration.Delegates;

public delegate ApplyDecorator<TService> ApplyDecoratorFactory<TService>(IServiceProvider provider);
