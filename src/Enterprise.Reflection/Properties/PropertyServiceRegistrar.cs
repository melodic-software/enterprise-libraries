using Enterprise.DI.Core.Registration.Abstract;
using Enterprise.Reflection.Properties.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Reflection.Properties;

internal sealed class PropertyServiceRegistrar : IRegisterServices
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IPropertyExistenceService>(provider => new PropertyExistenceService());
    }
}
