namespace Enterprise.DesignPatterns.ChainOfResponsibility.Classic.Handlers;

public interface IHandler<TRequest>
{
    void SetNext(IHandler<TRequest> next);
    void Handle(TRequest request);
}

public interface IHandler<TRequest, TResponse>
{
    void SetNext(IHandler<TRequest, TResponse?> next);
    TResponse? Handle(TRequest request);
}

public abstract class Handler<TRequest> : IHandler<TRequest>
{
    private IHandler<TRequest>? Next { get; set; }

    public void SetNext(IHandler<TRequest> next)
    {
        Next = next;
    }

    public void Handle(TRequest request)
    {
        Next?.Handle(request);
    }
}

public abstract class Handler<TRequest, TResponse> : IHandler<TRequest, TResponse>
{
    private IHandler<TRequest, TResponse?>? Next { get; set; }

    public void SetNext(IHandler<TRequest, TResponse?> next)
    {
        Next = next;
    }

    public TResponse? Handle(TRequest request)
    {
        return Next == null ? default : Next.Handle(request);
    }
}
