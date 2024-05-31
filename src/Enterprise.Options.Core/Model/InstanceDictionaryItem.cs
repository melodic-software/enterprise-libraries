namespace Enterprise.Options.Core.Model;

internal sealed class OptionsInstanceDictionaryItem
{
    public object? Options { get; }

    /// <summary>
    /// When locked, this options instance can no longer be reconfigured.
    /// Updates to the config after this point must be done via an IOptionsUpdater&lt;T&gt; OR directly via DynamicOptionsMonitor&lt;T&gt;.
    /// </summary>
    public bool IsLocked { get; }

    private OptionsInstanceDictionaryItem(object? options, bool isLocked)
    {
        Options = options;
        IsLocked = isLocked;
    }

    internal static OptionsInstanceDictionaryItem New(object? options) => new(options, false);
    internal static OptionsInstanceDictionaryItem Locked(object? options) => new(options, true);
}
