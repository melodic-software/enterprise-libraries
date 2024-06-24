using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Chains.RequestResponse;

namespace Enterprise.ApplicationServices.DI.Queries.Handlers.ChainOfResponsibility.Delegates;

public delegate void ConfigureChainOfResponsibility<TQuery, TResult>(ResponsibilityChainRegistrationBuilder<TQuery, TResult> builder);
