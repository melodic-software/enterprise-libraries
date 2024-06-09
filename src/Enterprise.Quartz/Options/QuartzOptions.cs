using Quartz;

namespace Enterprise.Quartz.Options;

public class QuartzOptions
{
    public const string ConfigSectionKey = "Custom:Quartz";

    public bool EnableQuartz { get; set; }
    public Action<IServiceCollectionQuartzConfigurator>? Configure { get; set; }
}
