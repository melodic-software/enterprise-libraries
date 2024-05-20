using Enterprise.DI.Core.Registration;
using Enterprise.Reflection.Properties;
using Enterprise.Reflection.Properties.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Reflection.Dependencies;

internal sealed class ReflectionServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IPropertyExistenceService>(provider => new PropertyExistenceService());
    }
}
