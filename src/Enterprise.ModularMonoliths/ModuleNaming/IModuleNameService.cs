namespace Enterprise.ModularMonoliths.ModuleNaming;

public interface IModuleNameService
{
    public string GetModuleName(Type type);
}
