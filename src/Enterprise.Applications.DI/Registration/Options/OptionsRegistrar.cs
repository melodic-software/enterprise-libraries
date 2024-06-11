using System.Reflection;
using Enterprise.Applications.DI.Registration.Methods;
using Enterprise.Logging.Core.Loggers;
using Enterprise.Options.Core.Abstract;
using Enterprise.Reflection.Assemblies.Delegates;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Enterprise.Applications.DI.Registration.Options;

public static class OptionsRegistrar
{
    /// <summary>
    /// Automatically resolves instances of <see cref="IRegisterOptions"/> and invokes the registration method.
    /// This allows for automatic wiring up of options in the DI container.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <param name="getAssemblies"></param>
    public static void AutoRegisterOptions(this IServiceCollection services, IConfiguration configuration, GetAssemblies? getAssemblies = null)
    {
        PreStartupLogger.Instance.LogInformation("Auto registering options with the DI container.");

        RegistrationMethodInvocationService.InvokeUsing(
            new RegistrationMethodConfig(
                typeof(IRegisterOptions),
                nameof(IRegisterOptions.RegisterOptions),
                MethodParamsAreValid,
                [services, configuration],
                getAssemblies: getAssemblies
            )
        );

        PreStartupLogger.Instance.LogInformation("Auto option registration complete.");
    }

    private static bool MethodParamsAreValid(ParameterInfo[] methodParameters) =>
        methodParameters.Length == 2 &&
        methodParameters[0].ParameterType == typeof(IServiceCollection) &&
        methodParameters[1].ParameterType == typeof(IConfiguration);
}
