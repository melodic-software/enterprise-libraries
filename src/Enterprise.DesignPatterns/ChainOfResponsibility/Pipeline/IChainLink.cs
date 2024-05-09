using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline;

/// <summary>
/// Represents a single link in a chain of responsibility that processes a request.
/// </summary>
/// <typeparam name="TRequest">The type of the request processed by this link.</typeparam>
public interface IChainLink<in TRequest>
{
    /// <summary>
    /// Executes this link in the chain.
    /// </summary>
    /// <param name="request">The request to process.</param>
    /// <param name="next">The delegate representing the next link in the chain.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task ExecuteAsync(TRequest request, NextChainLinkDelegate next, CancellationToken cancellationToken);
}

/// <summary>
/// Represents a single link in a chain of responsibility that processes a request and provides a response.
/// </summary>
/// <typeparam name="TRequest">The type of the request processed by this link.</typeparam>
/// <typeparam name="TResponse">The type of the response returned by this link.</typeparam>
public interface IChainLink<in TRequest, TResponse>
{
    /// <summary>
    /// Executes this link in the chain.
    /// </summary>
    /// <param name="request">The request to process.</param>
    /// <param name="next">The delegate representing the next link in the chain.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation and contains the response from this link or from subsequent links.</returns>
    Task<TResponse?> ExecuteAsync(TRequest request, NextChainLinkDelegate<TResponse> next, CancellationToken cancellationToken);
}
