using System.Reflection;

namespace Enterprise.Reflection.Assemblies.Delegates;

public delegate List<TypeInfo> GetAssemblyTypes(Assembly assembly, Type interfaceType);
