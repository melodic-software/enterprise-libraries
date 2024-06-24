namespace Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Chains.RequestOnly;

/// <summary>
/// Represents a responsibility chain for processing requests without producing a response.
/// </summary>
/// <typeparam name="TRequest">The type of request the chain will handle.</typeparam>
public interface IResponsibilityChain<in TRequest>
{
    /// <summary>
    /// Asynchronously executes the chain of responsibility for a given request.
    /// </summary>
    /// <param name="request">The request to process.</param>
    /// <param name="cancellationToken">Token for handling cancellation of the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}
