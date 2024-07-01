using Enterprise.Api.Startup.Options;
using Enterprise.Api.Startup.Options.Abstract;
using Enterprise.MediatR.Behaviors;
using Enterprise.MediatR.Behaviors.Caching;
using Enterprise.MediatR.Behaviors.ErrorHandling;
using Enterprise.MediatR.Behaviors.Logging;
using Enterprise.MediatR.Behaviors.Validation;
using Enterprise.ModularMonoliths.Options;
using Enterprise.Options.Core.Services.Singleton;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Api.MediatR;

internal sealed class MediatRBehaviorConfigurationService : IConfigureWebApiOptions
{
    public static void Configure(WebApiOptions options, IConfiguration configuration)
    {
        ModularMonolithOptions modularMonolithOptions = OptionsInstanceService.Instance
            .GetOptionsInstance<ModularMonolithOptions>(configuration, ModularMonolithOptions.ConfigSectionKey);

        options.ConfigureMediatR(mediatROptions =>
        {
            if (mediatROptions.BehaviorRegistrations.Any())
            {
                // We don't want to override any that have already been configured
                return;
            }

            var defaultRegistrations = new List<BehaviorRegistration>();

            if (modularMonolithOptions.EnableModularMonolith)
            {
                defaultRegistrations.Add(new(typeof(ModularMonolithRequestLoggingBehavior<,>), ServiceLifetime.Scoped));
            }
            
            defaultRegistrations.Add(new(typeof(RequestLoggingBehavior<,>), ServiceLifetime.Scoped));
            defaultRegistrations.Add(new(typeof(RequestExceptionHandlingBehavior<,>)));
            defaultRegistrations.Add(new(typeof(NullRequestValidationBehavior<,>)));
            defaultRegistrations.Add(new(typeof(UseCaseFluentValidationBehavior<,>)));

            // These apply to specific requests.
            defaultRegistrations.Add(new(typeof(QueryCachingBehavior<,>)));

            mediatROptions.BehaviorRegistrations = defaultRegistrations;
        });
    }
}
