using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.DI.Registration.Abstract;

public interface IRegisterServices
{
    public static abstract void RegisterServices(IServiceCollection services, IConfiguration configuration);
}
