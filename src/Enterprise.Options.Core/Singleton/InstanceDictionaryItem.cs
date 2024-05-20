namespace Enterprise.Options.Core.Singleton;

internal sealed class OptionsInstanceDictionaryItem
{
    public object? Options { get; }
    public bool IsLocked { get; }

    private OptionsInstanceDictionaryItem(object? options, bool isLocked)
    {
        Options = options;
        IsLocked = isLocked;
    }

    internal static OptionsInstanceDictionaryItem New(object? options) => new(options, false);
    internal static OptionsInstanceDictionaryItem Locked(object? options) => new(options, true);
}