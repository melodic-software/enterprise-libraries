using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.DI.Core.Registration.Abstract;

/// <summary>
/// Registers services with the DI container.
/// </summary>
public interface IRegisterServices
{
    /// <summary>
    /// Register services with the DI container.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static abstract void RegisterServices(IServiceCollection services, IConfiguration configuration);
}
