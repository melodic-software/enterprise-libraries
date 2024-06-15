using System.Collections.Concurrent;
using System.Reflection;

namespace Enterprise.AutoMapper.Assemblies;

public sealed class AutoMapperAssemblyService
{
    private static readonly Lazy<AutoMapperAssemblyService> Lazy = new(() => new AutoMapperAssemblyService());
    public static AutoMapperAssemblyService Instance => Lazy.Value;

    private readonly ConcurrentDictionary<string, Assembly> _assemblyDictionary = new();

    /// <summary>
    /// This is a collection of assemblies that contain mapping profiles.
    /// If there are no assemblies added to this collection, a fallback will be used that loads solution assemblies.
    /// </summary>
    internal List<Assembly> AssembliesToRegister => _assemblyDictionary
        .Select(x => x.Value)
        .OrderBy(x => x.GetName().FullName)
        .ToList();

    private AutoMapperAssemblyService() { }

    /// <summary>
    /// Explicitly add an assembly containing one or more mapping profiles.
    /// This will be included when AutoMapper services are registered.
    /// </summary>
    /// <param name="assembly"></param>
    public void AddAssembly(Assembly assembly)
    {
        string key = GetKey(assembly);
        _assemblyDictionary.TryAdd(key, assembly);
    }

    /// <summary>
    /// Explicitly remove an assembly containing one or more mapping profiles.
    /// This will NOT be included when AutoMapper services are registered.
    /// </summary>
    /// <param name="assembly"></param>
    public void RemoveAssembly(Assembly assembly)
    {
        string key = GetKey(assembly);
        _assemblyDictionary.TryRemove(key, out _);
    }

    private static string GetKey(Assembly assembly) => assembly.GetName().FullName;
}
