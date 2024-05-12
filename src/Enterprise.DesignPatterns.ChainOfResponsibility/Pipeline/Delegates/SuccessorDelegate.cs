namespace Enterprise.DesignPatterns.ChainOfResponsibility.Pipeline.Delegates;

/// <summary>
/// Represents a delegate for a successor handler in the chain that performs asynchronous operations without returning a value.
/// </summary>
public delegate Task SuccessorDelegate();

/// <summary>
/// Represents a delegate for a successor handler in the chain that performs asynchronous operations and returns a value of type TResponse.
/// </summary>
/// <typeparam name="TResponse">The type of response returned by the handler.</typeparam>
public delegate Task<TResponse> SuccessorDelegate<TResponse>();
