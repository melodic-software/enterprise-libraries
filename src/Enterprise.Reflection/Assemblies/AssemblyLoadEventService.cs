namespace Enterprise.Reflection.Assemblies;

/// <summary>
/// Provides the ability to attach event handlers to the assembly load event.
/// </summary>
public class AssemblyLoadEventService
{
    /// <summary>
    /// Attaches a handler to the event that is fired when an assembly is loaded for the first time.
    /// Since assemblies are lazily loaded, this is a way to execute some behavior for a specific assembly at the time it is loaded.
    /// Some assemblies may have already been loaded, depending on when this is called.
    /// </summary>
    /// <param name="eventHandler">The event handler to attach.</param>
    public static void HandleOnAssemblyLoad(AssemblyLoadEventHandler eventHandler)
    {
        AppDomain currentDomain = AppDomain.CurrentDomain;
        currentDomain.AssemblyLoad += eventHandler;
    }
}