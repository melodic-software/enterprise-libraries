using System.Reflection;
using Enterprise.ModularMonoliths.Attributes;
using Enterprise.ModularMonoliths.Options;
using Microsoft.Extensions.Options;

namespace Enterprise.ModularMonoliths.ModuleNaming;

public class ModuleNameService : IModuleNameService
{
    private readonly ModuleNamingOptions _options;

    public ModuleNameService(IOptions<ModuleNamingOptions> options)
    {
        _options = options.Value;
    }

    public string GetModuleName(Type type)
    {
        if (_options.UseModuleAttributes)
        {
            Assembly assembly = type.Assembly;
            object[] assemblyAttributes = assembly.GetCustomAttributes(typeof(ModuleAttribute), false);

            if (assemblyAttributes.FirstOrDefault() is ModuleAttribute moduleAttribute)
            {
                return moduleAttribute.ModuleName;
            }
        }

        if (_options.UseExplicitModuleFormat)
        {
            return type.FullName!.Split('.')[2];
        }

        if (_options.UseTruncatedModuleFormat)
        {
            return type.FullName!.Split('.')[0];
        }

        return null;
    }
}
