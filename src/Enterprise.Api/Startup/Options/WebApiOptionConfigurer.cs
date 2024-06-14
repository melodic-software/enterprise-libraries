using System.Reflection;
using Enterprise.Api.Startup.Options.Abstract;
using Enterprise.Applications.DI.Registration.Methods;
using Enterprise.Logging.Core.Loggers;
using Enterprise.Reflection.Assemblies.Delegates;
using Microsoft.Extensions.Logging;

namespace Enterprise.Api.Startup.Options;

public static class WebApiOptionConfigurer
{
    /// <summary>
    /// Dynamically resolves instances of <see cref="IConfigureWebApiOptions"/> and invokes the configure method.
    /// This allows for automatic wiring up of API configuration and keeps that concern separated.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="getAssemblies"></param>
    public static void Configure(WebApiOptions options, GetAssemblies? getAssemblies = null)
    {
        PreStartupLogger.Instance.LogInformation("Applying dynamic configuration for web API options.");

        RegistrationMethodInvocationService.InvokeUsing(
            new RegistrationMethodConfig(
                typeof(IConfigureWebApiOptions),
                nameof(IConfigureWebApiOptions.Configure),
                MethodParamsAreValid,
                [options],
                getAssemblies: getAssemblies
            )
        );

        PreStartupLogger.Instance.LogInformation("Dynamic configuration of web API option config delegates complete.");
    }

    private static bool MethodParamsAreValid(ParameterInfo[] methodParameters) =>
        methodParameters.Length == 1 &&
        methodParameters[0].ParameterType == typeof(WebApiOptions);
}
