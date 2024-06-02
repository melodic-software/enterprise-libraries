using Enterprise.Options.Core.Services.Singleton;
using Enterprise.Quartz.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Enterprise.Quartz.Config;

// https://www.quartz-scheduler.net
// https://www.milanjovanovic.tech/blog/scheduling-background-jobs-with-quartz-net

public static class QuartzConfigService
{
    public static void ConfigureQuartz(this IServiceCollection services, IConfiguration configuration)
    {
        QuartzConfigOptions quartzConfigOptions = OptionsInstanceService.Instance
            .GetOptionsInstance<QuartzConfigOptions>(configuration, QuartzConfigOptions.ConfigSectionKey);

        if (!quartzConfigOptions.EnableQuartz)
        {
            return;
        }

        quartzConfigOptions.Configure ??= DefaultConfigure;

        services.AddQuartz(quartzConfigOptions.Configure);

        services.AddQuartzHostedService(options =>
        {
            options.AwaitApplicationStarted = true;
            options.WaitForJobsToComplete = true;
            options.StartDelay = null;
        });
    }

    private static void DefaultConfigure(IServiceCollectionQuartzConfigurator configure)
    {
        // No configuration by default.
    }
}
