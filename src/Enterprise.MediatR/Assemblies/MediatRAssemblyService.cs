using System.Collections.Concurrent;
using System.Reflection;

namespace Enterprise.MediatR.Assemblies;

public sealed class MediatRAssemblyService
{
    private static readonly Lazy<MediatRAssemblyService> Lazy = new(() => new MediatRAssemblyService());
    public static MediatRAssemblyService Instance => Lazy.Value;

    private readonly ConcurrentDictionary<string, Assembly> _assemblyDictionary = new();

    /// <summary>
    /// This is a collection of assemblies that contain MediatR types.
    /// If there are no assemblies added to this collection, a fallback will be used that loads solution assemblies.
    /// </summary>
    internal List<Assembly> AssembliesToRegister => _assemblyDictionary
        .Select(x => x.Value)
        .OrderBy(x => x.GetName().FullName)
        .ToList();

    private MediatRAssemblyService() { }

    /// <summary>
    /// Explicitly add an assembly containing MediatR types.
    /// This will be included when MediatR services are registered.
    /// </summary>
    /// <param name="assembly"></param>
    public void AddAssembly(Assembly assembly)
    {
        string key = GetKey(assembly);
        _assemblyDictionary.TryAdd(key, assembly);
    }

    /// <summary>
    /// Explicitly remove an assembly containing one or more MediatR types.
    /// This will be excluded when MediatR services are registered.
    /// </summary>
    /// <param name="assembly"></param>
    public void RemoveAssembly(Assembly assembly)
    {
        string key = GetKey(assembly);
        _assemblyDictionary.TryRemove(key, out _);
    }

    private static string GetKey(Assembly assembly) => assembly.GetName().FullName;
}
