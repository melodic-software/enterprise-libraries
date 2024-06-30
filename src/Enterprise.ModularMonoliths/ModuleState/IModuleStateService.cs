namespace Enterprise.ModularMonoliths.ModuleState;

public interface IModuleStateService
{
    bool ModuleEnabled(string moduleName);
    bool ModuleEnabled(Type type);
}
