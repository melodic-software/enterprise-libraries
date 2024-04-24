using Enterprise.DI.Core.Registration;
using Enterprise.Logging.Core.Loggers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Enterprise.Applications.DI.Registration.Services;

public static class ServiceRegistrar
{
    /// <summary>
    /// Automatically resolves instances of <see cref="IRegisterServices"/> and invokes the registration method.
    /// This allows for automatic wiring up of services in the DI container.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="getAssemblies"></param>
    public static void AutoRegisterServices(this IServiceCollection services, IConfiguration configuration, Func<Assembly[]>? getAssemblies = null)
    {
        PreStartupLogger.Instance.LogInformation("Auto registering services with the DI container.");

        RegistrationMethodInvocationService.InvokeUsing(
            new RegistrationMethodConfig(
                typeof(IRegisterServices),
                nameof(IRegisterServices.RegisterServices),
                MethodParamsAreValid,
                [services, configuration],
                getAssemblies: getAssemblies
            )
        );

        PreStartupLogger.Instance.LogInformation("Auto service registration complete.");
    }

    private static bool MethodParamsAreValid(ParameterInfo[] methodParameters) =>
        methodParameters.Length == 2 &&
        methodParameters[0].ParameterType == typeof(IServiceCollection) &&
        methodParameters[1].ParameterType == typeof(IConfiguration);
}