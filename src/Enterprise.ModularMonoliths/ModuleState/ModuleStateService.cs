using Enterprise.ModularMonoliths.ModuleNaming;
using Enterprise.ModularMonoliths.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Enterprise.ModularMonoliths.ModuleState;

public class ModuleStateService : IModuleStateService
{
    private readonly IConfiguration _configuration;
    private readonly IModuleNameService _moduleNameService;
    private readonly ModuleStateOptions _moduleStateOptions;

    public ModuleStateService(IConfiguration configuration, IOptions<ModuleStateOptions> moduleStateOptions, IModuleNameService moduleNameService)
    {
        _configuration = configuration;
        _moduleNameService = moduleNameService;
        _moduleStateOptions = moduleStateOptions.Value;
    }

    public bool ModuleEnabled(string moduleName)
    {
        IConfigurationSection configSection = _configuration.GetSection(moduleName);

        if (configSection.Value is null)
        {
            return false;
        }

        bool enabled = configSection.GetValue(_moduleStateOptions.ConfigSettingKeyName, false);

        return enabled;
    }

    public bool ModuleEnabled(Type type)
    {
        string? moduleName = _moduleNameService.GetModuleName(type);
        return !string.IsNullOrWhiteSpace(moduleName) && ModuleEnabled(moduleName);
    }
}
