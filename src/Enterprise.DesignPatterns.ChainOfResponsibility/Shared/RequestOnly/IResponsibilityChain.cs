using Enterprise.DesignPatterns.ChainOfResponsibility.Modern.Handlers.RequestOnly;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Shared.RequestOnly;

public interface IResponsibilityChain<in TRequest> : IHandle<TRequest> where TRequest : class;
