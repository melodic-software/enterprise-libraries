using Enterprise.Options.Core.Singleton;
using Enterprise.Quartz.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Enterprise.Quartz.Config;

public static class QuartzConfigService
{
    public static void ConfigureQuartz(this IServiceCollection services, IConfiguration configuration)
    {
        QuartzConfigOptions quartzConfigOptions = OptionsInstanceService.Instance
            .GetOptionsInstance<QuartzConfigOptions>(configuration, QuartzConfigOptions.ConfigSectionKey);

        if (!quartzConfigOptions.EnableQuartz)
            return;

        services.AddQuartz();
        services.AddQuartzHostedService(options =>
        {
            options.AwaitApplicationStarted = true;
            options.WaitForJobsToComplete = true;
            options.StartDelay = null;
        });
    }
}