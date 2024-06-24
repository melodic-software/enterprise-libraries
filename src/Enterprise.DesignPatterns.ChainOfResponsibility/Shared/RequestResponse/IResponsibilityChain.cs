using Enterprise.DesignPatterns.ChainOfResponsibility.Modern.Handlers.RequestResponse;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Shared.RequestResponse;

public interface IResponsibilityChain<in TRequest, out TResponse> : IHandle<TRequest, TResponse> where TRequest : class;
