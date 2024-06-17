namespace Enterprise.Api.Minimal.Options;

public class MinimalApiOptions
{
    public const string ConfigSectionKey = "Custom:MinimalApi";

    public bool EnableMinimalApiEndpoints { get; set; } = true;
}
