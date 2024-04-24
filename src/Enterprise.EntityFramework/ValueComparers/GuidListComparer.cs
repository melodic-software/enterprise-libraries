using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Enterprise.EntityFramework.ValueComparers;

public class GuidListComparer : ValueComparer<List<Guid>>
{
    public GuidListComparer() : base(
        (t1, t2) => t1!.SequenceEqual(t2!),
        t => t.Select(x => x.GetHashCode()).Aggregate((x, y) => x ^ y),
        t => t)
    {
    }
}