using Enterprise.DesignPatterns.ChainOfResponsibility.Modern.Handlers;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Shared;

public interface IResponsibilityChain<in TRequest> : IHandle<TRequest> where TRequest : class;
public interface IResponsibilityChain<in TRequest, out TResponse> : IHandle<TRequest, TResponse> where TRequest : class;
