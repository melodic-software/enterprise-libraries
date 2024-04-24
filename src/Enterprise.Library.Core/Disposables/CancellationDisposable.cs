namespace Enterprise.Library.Core.Disposables;

public class CancellationDisposable : IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource;

    public CancellationDisposable(CancellationTokenSource cancellationTokenSource)
    {
        _cancellationTokenSource = cancellationTokenSource;
    }

    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
    }

    public static IDisposable NoOp => new CancellationDisposable(CancellationTokenSource.CreateLinkedTokenSource(CancellationToken.None));
}