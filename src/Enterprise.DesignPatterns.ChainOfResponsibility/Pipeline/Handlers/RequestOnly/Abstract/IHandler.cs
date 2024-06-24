using Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;

namespace Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Handlers.RequestOnly.Abstract;

/// <summary>
/// Defines a handler in a responsibility chain for processing requests.
/// </summary>
/// <typeparam name="TRequest">The type of request to be processed.</typeparam>
public interface IHandler<in TRequest>
{
    /// <summary>
    /// Asynchronously processes a request.
    /// </summary>
    /// <param name="request">The request to process.</param>
    /// <param name="next">The next handler in the chain to be called after this handler.</param>
    /// <param name="cancellationToken">Token for handling cancellation of the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task HandleAsync(TRequest request, SuccessorDelegate next, CancellationToken cancellationToken = default);
}
