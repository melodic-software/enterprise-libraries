using Enterprise.Events.Callbacks.Model;

namespace Enterprise.Events.Callbacks.Registration.Abstract;

public interface IGetRegisteredCallbacks
{
    /// <summary>
    /// Get the current collection of callback registrations.
    /// </summary>
    /// <returns></returns>
    public Dictionary<Type, IEnumerable<IEventCallback>> GetRegisteredCallbacks();
}
