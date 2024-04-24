namespace Enterprise.Quartz.Options;

public class QuartzConfigOptions
{
    public const string ConfigSectionKey = "Custom:Quartz";

    public bool EnableQuartz { get; set; } = false;
}