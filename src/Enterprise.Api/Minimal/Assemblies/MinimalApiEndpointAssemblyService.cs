using System.Collections.Concurrent;
using System.Reflection;
using Enterprise.Api.Minimal.Mapping;

namespace Enterprise.Api.Minimal.Assemblies;

public class MinimalApiEndpointAssemblyService
{
    private static readonly Lazy<MinimalApiEndpointAssemblyService> Lazy = new(() => new MinimalApiEndpointAssemblyService());
    public static MinimalApiEndpointAssemblyService Instance => Lazy.Value;

    private readonly ConcurrentDictionary<string, Assembly> _assemblyDictionary = new();

    /// <summary>
    /// A collection of assemblies that contain implementations of <see cref="IMapEndpoint"/> or <see cref="IMapEndpoints"/>.
    /// If this collection is empty, a fallback will be provided that loads all solution assemblies.
    /// </summary>
    internal List<Assembly> AssembliesToRegister => _assemblyDictionary
        .Select(x => x.Value)
        .OrderBy(x => x.GetName().FullName)
        .ToList();

    private MinimalApiEndpointAssemblyService() { }

    /// <summary>
    /// Explicitly add an assembly containing implementations of <see cref="IMapEndpoint"/> or <see cref="IMapEndpoints"/>.
    /// These will be included when minimal API endpoints are registered.
    /// </summary>
    /// <param name="assembly"></param>
    public void AddAssembly(Assembly assembly)
    {
        string key = GetKey(assembly);
        _assemblyDictionary.TryAdd(key, assembly);
    }

    /// <summary>
    /// Explicitly remove an assembly containing implementations of <see cref="IMapEndpoint"/> or <see cref="IMapEndpoints"/>.
    /// This will be excluded when minimal API endpoints are registered.
    /// </summary>
    /// <param name="assembly"></param>
    public void RemoveAssembly(Assembly assembly)
    {
        string key = GetKey(assembly);
        _assemblyDictionary.TryRemove(key, out _);
    }

    private static string GetKey(Assembly assembly) => assembly.GetName().FullName;
}
