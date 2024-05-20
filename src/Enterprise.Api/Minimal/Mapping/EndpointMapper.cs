using System.Reflection;
using Enterprise.Reflection.Assemblies;
using Microsoft.AspNetCore.Builder;
using static Enterprise.Reflection.Assemblies.AssemblyFilterPredicates;
using static Enterprise.Reflection.Types.AssignableConcreteTypeService;

namespace Enterprise.Api.Minimal.Mapping;

public static class EndpointMapper
{
    public static void MapEndpoints(IApplicationBuilder app)
    {
        List<Assembly> assemblies = AssemblyLoader
            .LoadSolutionAssemblies(ThatAreNotMicrosoft)
            .ToList();

        MapEndpoints(app, assemblies);
    }

    public static void MapEndpoints(IApplicationBuilder app, IEnumerable<Assembly> assemblies)
    {
        List<TypeInfo> types = [];

        foreach (Assembly assembly in assemblies)
        {
            types.AddRange(GetAssignableConcreteTypes(assembly, typeof(IMapEndpoints)));
        }

        MapEndpoints(app, types);
    }

    public static void MapEndpoints<T>(this IApplicationBuilder app)
    {
        app.MapEndpoints(typeof(T));
    }

    public static void MapEndpoints(this IApplicationBuilder app, Type typeMarker)
    {
        List<TypeInfo> endpointTypes = GetEndpointTypes(typeMarker);

        MapEndpoints(app, endpointTypes);
    }

    private static List<TypeInfo> GetEndpointTypes(Type typeMarker) => GetAssignableConcreteTypes(typeMarker.Assembly, typeof(IMapEndpoints));

    private static void MapEndpoints(IApplicationBuilder app, List<TypeInfo> endpointTypes)
    {
        foreach (TypeInfo endpointType in endpointTypes)
        {
            string methodName = nameof(IMapEndpoints.MapEndpoints);
            MethodInfo? method = endpointType.GetMethod(methodName);
            method?.Invoke(null, [app]);
        }
    }
}
