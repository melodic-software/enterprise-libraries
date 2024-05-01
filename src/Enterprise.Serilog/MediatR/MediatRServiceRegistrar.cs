using Enterprise.MediatR.Behaviors.Logging.Services;
using Enterprise.MediatR.Behaviors.Logging.Services.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Enterprise.Serilog.MediatR;

public static class MediatRServiceRegistrar
{
    public static void RegisterMediatRServices(this IHostApplicationBuilder builder)
    {
        // TODO: We need to ensure there aren't conflicting registrations with other providers, etc.
        builder.Services.AddTransient(provider =>
        {
            ILoggingBehaviorService loggingBehaviorService = new LoggingBehaviorService();
            ILoggingBehaviorService serilogDecorator = new SerilogLoggingBehaviorServiceDecorator(loggingBehaviorService);
            return serilogDecorator;
        });
    }
}
