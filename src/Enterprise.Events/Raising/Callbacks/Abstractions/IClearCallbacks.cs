namespace Enterprise.Events.Raising.Callbacks.Abstractions;

public interface IClearCallbacks
{
    /// <summary>
    /// Clears any registered callbacks.
    /// </summary>
    public void ClearCallbacks();
}