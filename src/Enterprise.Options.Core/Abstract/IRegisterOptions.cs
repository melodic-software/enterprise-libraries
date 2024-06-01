using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Options.Core.Abstract;

public interface IRegisterOptions
{
    /// <summary>
    /// Register one or more options with the DI container.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static abstract void RegisterOptions(IServiceCollection services, IConfiguration configuration);
}
