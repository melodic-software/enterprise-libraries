using Microsoft.Extensions.Options;

namespace Enterprise.Options.Core.Abstract;

/// <summary>
/// Implementations apply updates to configuration options objects.
/// This is an alternative to updating the application settings (JSON file).
/// It allows for in memory updates that can be done any time after application startup.
/// The most common scenario would be real time config updates consumed and applied via messaging.
/// </summary>
/// <typeparam name="TOptions"></typeparam>
public interface IOptionsUpdater<out TOptions> : IOptionsSnapshot<TOptions> where TOptions : class, new()
{
    /// <summary>
    /// Update the options instance using the provided delegate.
    /// </summary>
    /// <param name="applyChanges"></param>
    void UpdateOptions(Action<TOptions> applyChanges);
}