namespace Enterprise.EventSourcing;

public interface ISnapshot
{
    /// <summary>
    /// Gets the version recorded at the time of the snapshot.
    /// </summary>
    int Version { get; }
}

/// <summary>
/// Represents a snapshot of state at a specific version.
/// </summary>
/// <typeparam name="TId">The type of the identifier.</typeparam>
public interface ISnapshot<out TId> : ISnapshot
{
    /// <summary>
    /// Gets the unique identifier.
    /// This is typically an aggregate root (entity) ID.
    /// </summary>
    TId Id { get; }
}