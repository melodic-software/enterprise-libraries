namespace Enterprise.DesignPatterns.ChainOfResponsibility.Modern.Handlers;

public interface ICouldHandle<in TRequest> : IHandlerBase where TRequest : class
{
    /// <summary>
    /// Each handler in the chain can inspect the request to see if it can be handled.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    bool CanHandle(TRequest request);
}
