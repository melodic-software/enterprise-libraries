namespace Enterprise.ModularMonoliths.Options;

public class ModularMonolithOptions
{
    public const string ConfigSectionKey = "Custom:ModularMonolith";

    public bool EnableModularMonolith { get; set; }

    /// <summary>
    /// The search pattern used for JSON files containing application settings.
    /// </summary>
    public string JsonSettingsFileNameSearchPattern { get; set; } = "modules.*";
}
