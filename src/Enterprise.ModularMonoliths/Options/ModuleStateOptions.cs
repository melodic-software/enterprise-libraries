namespace Enterprise.ModularMonoliths.Options;

public sealed class ModuleStateOptions
{
    public const string ConfigSectionKey = "Custom:ModularMonolith:ModuleState";

    /// <summary>
    /// This is the config setting that drives whether a module is enabled.
    /// If this is not configured or is mistyped, the default assumption is that the module is not enabled.
    /// </summary>
    public string ConfigSettingKeyName { get; set; } = "Enabled";
}
