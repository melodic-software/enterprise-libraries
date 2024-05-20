using System.Collections.Concurrent;
using Enterprise.Library.Core.Disposables;
using Microsoft.Extensions.Logging;

namespace Enterprise.Options;

internal sealed class ChangeNotifier<TOptions>
{
    private readonly ConcurrentDictionary<Guid, Action<TOptions, string?>> _subscriptions = new();
    private readonly ILogger _logger;

    internal ChangeNotifier(ILogger logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Subscribe to change notifications.
    /// The provided action will be executed when a change occurs.
    /// </summary>
    /// <param name="onChange"></param>
    /// <returns></returns>
    internal IDisposable Subscribe(Action<TOptions, string?> onChange)
    {
        var subscriptionId = Guid.NewGuid();
        _subscriptions.TryAdd(subscriptionId, onChange);
        return new DisposableAction(() => _subscriptions.TryRemove(subscriptionId, out _));
    }

    /// <summary>
    /// Notify subscribers that the options have changed.
    /// </summary>
    /// <param name="options">The current options value.</param>
    internal void NotifySubscribers(TOptions options)
    {
        _logger.LogInformation("Notifying subscribers that a config change has occurred.");

        foreach (Action<TOptions, string?> subscriber in _subscriptions.Values)
        {
            try
            {
                subscriber(options, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error has occurred during subscriber notification.");
            }
        }

        _logger.LogInformation("Completed notifying subscribers about config changes.");
    }
}
