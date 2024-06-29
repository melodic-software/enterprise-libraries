using Enterprise.DomainDrivenDesign.Entities.Standard;

namespace Enterprise.DomainDrivenDesign.Entities.Equality;

public static class EntityEqualityExtensions
{
    public static bool HasSameIdentifierAs<TId>(this Entity<TId> entity, Entity<TId> other)
        where TId : IEquatable<TId>
    {
        return entity.Id.Equals(other.Id);
    }
}
