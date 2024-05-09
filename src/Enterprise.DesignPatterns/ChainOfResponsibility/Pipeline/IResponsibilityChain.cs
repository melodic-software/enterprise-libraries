namespace Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline;

/// <summary>
/// Defines the responsibility for executing the entire chain of links for a request.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
public interface IResponsibilityChain<in TRequest>
{
    /// <summary>
    /// Executes the entire chain of responsibility for a given request.
    /// </summary>
    /// <param name="request">The request to process through the chain.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task ExecuteAsync(TRequest request, CancellationToken cancellationToken);
}

/// <summary>
/// Defines the responsibility for executing the entire chain of links for a request with a response.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public interface IResponsibilityChain<in TRequest, TResponse>
{
    /// <summary>
    /// Executes the entire chain of responsibility and returns a response.
    /// </summary>
    /// <param name="request">The request to process through the chain.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation and includes the response.</returns>
    Task<TResponse?> ExecuteAsync(TRequest request, CancellationToken cancellationToken);
}
