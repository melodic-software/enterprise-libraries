using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.DI.Core.Registration;

public interface IRegisterServices
{
    public static abstract void RegisterServices(IServiceCollection services, IConfiguration configuration);
}