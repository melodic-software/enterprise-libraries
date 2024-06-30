namespace Enterprise.ModularMonoliths.Attributes;

[AttributeUsage(AttributeTargets.Assembly)]
public sealed class ModuleAttribute : Attribute
{
    public string ModuleName { get; }

    public ModuleAttribute(string moduleName)
    {
        ModuleName = moduleName;
    }
}
