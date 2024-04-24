namespace Enterprise.Library.Core.Disposables;

public class DisposableAction : IDisposable
{
    private readonly Action _disposeAction;

    public DisposableAction(Action disposeAction)
    {
        _disposeAction = disposeAction ?? throw new ArgumentNullException(nameof(disposeAction));
    }

    public void Dispose()
    {
        _disposeAction();
    }

    // A static no-operation DisposableAction instance that does nothing on disposal.
    public static IDisposable NoOp => new DisposableAction(() => { });
}
