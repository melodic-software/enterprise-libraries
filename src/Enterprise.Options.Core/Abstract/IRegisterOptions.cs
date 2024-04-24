using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Options.Core.Abstract;

public interface IRegisterOptions
{
    public static abstract void RegisterOptions(IServiceCollection services, IConfiguration configuration);
}