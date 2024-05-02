using Enterprise.Events.Raising.Callbacks.Model;

namespace Enterprise.Events.Raising.Callbacks.Abstractions;

public interface IGetRegisteredCallbacks
{
    /// <summary>
    /// Get the current collection of callback registrations.
    /// </summary>
    /// <returns></returns>
    public Dictionary<Type, IEnumerable<IEventCallback>> GetRegisteredCallbacks();
}