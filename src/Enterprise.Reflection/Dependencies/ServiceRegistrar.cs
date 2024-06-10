using Enterprise.DI.Core.Registration.Abstract;
using Enterprise.Reflection.Properties;
using Enterprise.Reflection.Properties.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Reflection.Dependencies;

internal sealed class ServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IPropertyExistenceService>(provider => new PropertyExistenceService());
    }
}
