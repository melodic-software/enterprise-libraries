using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace Enterprise.Applications.DI.Startup;

public class SharedFrameworkAssemblyService
{
    private static readonly Lazy<SharedFrameworkAssemblyService> Lazy = new();
    private readonly ConcurrentDictionary<string, byte> _sharedFrameworkDirectories = new();

    public static SharedFrameworkAssemblyService Instance = Lazy.Value;

    public IReadOnlyCollection<string> SharedFrameworkDirectories =>
        _sharedFrameworkDirectories.Select(x => x.Key).ToImmutableList();

    public void AddSharedDirectory(string? directory)
    {
        if (string.IsNullOrWhiteSpace(directory))
            return;

        if (_sharedFrameworkDirectories.ContainsKey(directory))
            return;

        _sharedFrameworkDirectories.TryAdd(directory, byte.MinValue);
    }

    public void AddSharedDirectories(string?[] directories)
    {
        foreach (string? directory in directories)
            AddSharedDirectory(directory);
    }

    public void RemoveSharedDirectory(string? directory)
    {
        if (string.IsNullOrWhiteSpace(directory))
            return;

        if (!_sharedFrameworkDirectories.ContainsKey(directory))
            return;

        _sharedFrameworkDirectories.TryRemove(directory, out byte value);
    }

    public void ClearDirectories()
    {
        _sharedFrameworkDirectories.Clear();
    }
}