namespace Enterprise.Events.Services.Raising.Callbacks.Abstractions;

public interface IClearCallbacks
{
    /// <summary>
    /// Clears any registered callbacks.
    /// </summary>
    public void ClearCallbacks();
}