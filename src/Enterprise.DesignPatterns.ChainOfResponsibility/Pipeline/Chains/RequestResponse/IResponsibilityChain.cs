namespace Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Chains.RequestResponse;

/// <summary>
/// Represents a responsibility chain for processing requests and producing a response.
/// </summary>
/// <typeparam name="TRequest">The type of request the chain will handle.</typeparam>
/// <typeparam name="TResponse">The type of response the chain will produce.</typeparam>
public interface IResponsibilityChain<in TRequest, TResponse>
{
    /// <summary>
    /// Asynchronously executes the chain of responsibility for a given request and produces a response.
    /// </summary>
    /// <param name="request">The request to process.</param>
    /// <param name="cancellationToken">Token for handling cancellation of the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the response.</returns>
    Task<TResponse?> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}
