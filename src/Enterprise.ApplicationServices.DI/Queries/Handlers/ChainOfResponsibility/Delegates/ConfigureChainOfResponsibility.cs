using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Dependencies;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.ChainOfResponsibility.Delegates;

public delegate void ConfigureChainOfResponsibility<TQuery, TResult>(ResponsibilityChainRegistrationBuilder<TQuery, TResult> builder);
