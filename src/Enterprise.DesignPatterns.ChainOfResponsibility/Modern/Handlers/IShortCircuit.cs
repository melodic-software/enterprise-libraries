namespace Enterprise.DesignPatterns.ChainOfResponsibility.Modern.Handlers;

public interface IShortCircuit : IHandlerBase
{
    /// <summary>
    /// Determines if the chain of responsibility should be short-circuited.
    /// With the classic form, this is typically done after the first handler in the chain is able to handle the request.
    /// </summary>
    public bool ShortCircuit { get; }
}
