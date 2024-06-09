using Enterprise.Options.Core.Services.Singleton;
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
        Options.QuartzOptions options = OptionsInstanceService.Instance
            .GetOptionsInstance<Options.QuartzOptions>(configuration, Options.QuartzOptions.ConfigSectionKey);

        if (!options.EnableQuartz)
        {
            return;
        }

        options.Configure ??= _ => { };

        services.AddQuartz(options.Configure);

        services.AddQuartzHostedService(quartzHostedServiceOptions =>
        {
            quartzHostedServiceOptions.AwaitApplicationStarted = true;
            quartzHostedServiceOptions.WaitForJobsToComplete = true;
            quartzHostedServiceOptions.StartDelay = null;
        });
    }
}
