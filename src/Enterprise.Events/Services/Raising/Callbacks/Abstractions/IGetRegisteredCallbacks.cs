using Enterprise.Events.Services.Raising.Callbacks.Model;

namespace Enterprise.Events.Services.Raising.Callbacks.Abstractions;

public interface IGetRegisteredCallbacks
{
    /// <summary>
    /// Get the current collection of callback registrations.
    /// </summary>
    /// <returns></returns>
    public Dictionary<Type, IEnumerable<IEventCallback>> GetRegisteredCallbacks();
}