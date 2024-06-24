using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers.RequestResponse.Abstract;

/// <summary>
/// Defines a handler in a responsibility chain for processing requests and producing a response.
/// </summary>
/// <typeparam name="TRequest">The type of request to be processed.</typeparam>
/// <typeparam name="TResponse">The type of response to be produced.</typeparam>
public interface IHandler<in TRequest, TResponse>
{
    /// <summary>
    /// Asynchronously processes a request and produces a response.
    /// </summary>
    /// <param name="request">The request to process.</param>
    /// <param name="next">The next handler in the chain to be called after this handler.</param>
    /// <param name="cancellationToken">Token for handling cancellation of the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the response.</returns>
    Task<TResponse?> HandleAsync(TRequest request, SuccessorDelegate<TResponse> next, CancellationToken cancellationToken = default);
}
