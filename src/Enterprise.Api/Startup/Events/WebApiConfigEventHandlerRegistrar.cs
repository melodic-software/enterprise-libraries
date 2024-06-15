using System.Reflection;
using Enterprise.Api.Startup.Events.Abstract;
using Enterprise.Applications.DI.Registration.Methods;
using Enterprise.Logging.Core.Loggers;
using Enterprise.Reflection.Assemblies.Delegates;
using Microsoft.Extensions.Logging;

namespace Enterprise.Api.Startup.Events;

public static class WebApiConfigEventHandlerRegistrar
{
    /// <summary>
    /// Dynamically resolves instances of <see cref="IRegisterWebApiConfigEventHandlers"/> and invokes the configure method.
    /// This allows for automatic wiring up of API config event handlers and keeps that concern separated.
    /// </summary>
    /// <param name="events"></param>
    /// <param name="getAssemblies"></param>
    public static void RegisterHandlers(WebApiConfigEvents events, GetAssemblies? getAssemblies = null)
    {
        PreStartupLogger.Instance.LogInformation("Dynamically registering event handlers for API config events.");

        RegistrationMethodInvocationService.InvokeUsing(
            new RegistrationMethodConfig(
                typeof(IRegisterWebApiConfigEventHandlers),
                nameof(IRegisterWebApiConfigEventHandlers.RegisterHandlers),
                MethodParamsAreValid,
                [events],
                getAssemblies: getAssemblies
            )
        );

        PreStartupLogger.Instance.LogInformation("API config event handler registration complete.");
    }

    private static bool MethodParamsAreValid(ParameterInfo[] methodParameters) =>
        methodParameters.Length == 1 &&
        methodParameters[0].ParameterType == typeof(WebApiConfigEvents);
}
