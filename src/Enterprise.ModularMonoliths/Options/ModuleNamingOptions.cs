namespace Enterprise.ModularMonoliths.Options;

public sealed class ModuleNamingOptions
{
    public const string ConfigSectionKey = "Custom:ModularMonolith:ModuleNaming";

    /// <summary>
    /// This means all module projects should follow an explicit format.
    /// The module name is contained in the third segment.
    /// Ex: "Evently.Modules.Events.Domain".
    /// </summary>
    public bool UseExplicitModuleFormat { get; set; }

    /// <summary>
    /// This means all module projects do not start with the name of the bootstrapping application.
    /// This also means the explicit word "module" is not used.
    /// It is expected for the module name to be the first segment.
    /// EX: "Events.Domain".
    /// </summary>
    public bool UseTruncatedModuleFormat { get; set; } = true;

    /// <summary>
    /// This will search for any assembly attributes when resolving module name association.
    /// </summary>
    public bool UseModuleAttributes { get; set; } = true;
}
