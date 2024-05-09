namespace Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;

/// <summary>
/// Represents an async continuation for the next link to execute in the chain.
/// </summary>
/// <returns></returns>
public delegate Task NextChainLinkDelegate();

/// <summary>
/// Represents an async continuation for the next link to execute in the chain.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
/// <returns></returns>
public delegate Task<TResponse> NextChainLinkDelegate<TResponse>();
