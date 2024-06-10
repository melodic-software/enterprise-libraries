using System.Collections.Concurrent;
using Enterprise.Library.Core.Disposables;
using Enterprise.Options.Monitoring.ChangeNotification.Delegates;
using Microsoft.Extensions.Logging;

namespace Enterprise.Options.Monitoring.ChangeNotification;

internal sealed class ChangeNotifier<TOptions>
{
    private readonly ConcurrentDictionary<Guid, OnChange<TOptions>> _subscriptions = new();
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
    internal IDisposable Subscribe(OnChange<TOptions> onChange)
    {
        var subscriptionId = Guid.NewGuid();
        _subscriptions.TryAdd(subscriptionId, onChange.Invoke);
        return new DisposableAction(() => _subscriptions.TryRemove(subscriptionId, out _));
    }

    /// <summary>
    /// Notify subscribers that the options have changed.
    /// </summary>
    /// <param name="options">The current options value.</param>
    internal void NotifySubscribers(TOptions options)
    {
        _logger.LogInformation("Notifying subscribers that a config change has occurred.");

        foreach (OnChange<TOptions> subscriber in _subscriptions.Values)
        {
            try
            {
                subscriber(options);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error has occurred during subscriber notification.");
            }
        }

        _logger.LogInformation("Completed notifying subscribers about config changes.");
    }
}
