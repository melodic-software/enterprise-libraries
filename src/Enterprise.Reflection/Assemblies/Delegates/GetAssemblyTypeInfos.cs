using System.Reflection;

namespace Enterprise.Reflection.Assemblies.Delegates;

public delegate List<TypeInfo> GetAssemblyTypeInfos(Assembly assembly, Type interfaceType);
