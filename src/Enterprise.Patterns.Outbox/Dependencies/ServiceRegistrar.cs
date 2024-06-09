using Enterprise.DateTimes.Current.Abstract;
using Enterprise.DI.Core.Registration;
using Enterprise.Patterns.Outbox.Factory;
using Enterprise.Patterns.Outbox.Serializers;
using Enterprise.Serialization.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Patterns.Outbox.Dependencies;

internal sealed class ServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(provider =>
        {
            ISerializeJson jsonSerializer = new OutboxMessageContentSerializer();
            IDateTimeUtcNowProvider dateTimeProvider = provider.GetRequiredService<IDateTimeUtcNowProvider>();
            var outboxMessageFactory = new EventOutboxMessageFactory(jsonSerializer, dateTimeProvider);
            return outboxMessageFactory;
        });
    }
}
