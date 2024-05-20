namespace Enterprise.ValueObjects.Model;

// Alternatives
// https://enterprisecraftsmanship.com/posts/value-object-better-implementation
// https://github.com/ardalis/Ardalis.SharedKernel/blob/main/src/Ardalis.SharedKernel/ValueObject.cs

/// <summary>
/// An immutable object with no conceptual identity that is defined solely by properties and their values.
/// Two value objects are equal if their values are the same (structural equality).
/// They encapsulate primitive types and combats primitive obsession, promoting more expressive code.
/// Child classes should ensure that all properties are immutable and that equality checks consider the structural characteristics of complex and nested objects.
/// NOTE: For most cases in C# 10+, consider using `readonly record struct` for simpler value object implementations.
/// See: https://nietras.com/2021/06/14/csharp-10-record-struct/
/// </summary>
public abstract class ValueObject : IEquatable<ValueObject>, IEqualityComparer<ValueObject>, IComparable, IComparable<ValueObject>
{
    private readonly Lazy<int> _cachedHashCode;

    /// <summary>
    /// Initializes a new instance of the <see cref="ValueObject"/> class.
    /// Using Lazy&lt;T&gt; for hash code computation defers the calculation until it is actually needed
    /// and caches the result, enhancing performance for objects that are compared frequently.
    /// </summary>
    protected ValueObject()
    {
        _cachedHashCode = new Lazy<int>(ComputeHashCode);
    }

    /// <summary>
    /// Equality operator override.
    /// Returns true if both value objects are equal.
    /// </summary>
    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        if (left is null && right is null)
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        return left.Equals(right);
    }

    /// <summary>
    /// Inequality operator override.
    /// Returns true if value objects are not equal.
    /// </summary>
    public static bool operator !=(ValueObject? left, ValueObject? right) => !(left == right);

    public static bool operator <(ValueObject? left, ValueObject? right) =>
        left is null ? right is not null : left.CompareTo(right) < 0;

    public static bool operator <=(ValueObject? left, ValueObject? right) =>
        left is null || left.CompareTo(right) <= 0;

    public static bool operator >(ValueObject? left, ValueObject? right) =>
        left is not null && left.CompareTo(right) > 0;

    public static bool operator >=(ValueObject? left, ValueObject? right) =>
        left is null ? right is null : left.CompareTo(right) >= 0;

    /// <inheritdoc />
    public int CompareTo(ValueObject? other)
    {
        return CompareTo(other as object);
    }

    /// <inheritdoc />
    public int CompareTo(object? obj)
    {
        if (obj == null)
        {
            return 1;
        }

        // TODO: If ORMs are heavily used and proxy objects are a concern, consider revisiting this.
        // Functionality may need to be added to get the "unproxied" type.

        Type thisType = GetType();
        Type otherType = obj.GetType();

        if (thisType != otherType)
        {
            return string.Compare(thisType.ToString(), otherType.ToString(), StringComparison.Ordinal);
        }

        var other = (ValueObject)obj;

        object?[] components = GetEqualityComponents().ToArray();
        object?[] otherComponents = other.GetEqualityComponents().ToArray();

        for (int i = 0; i < components.Length; i++)
        {
            int comparison = CompareComponents(components[i], otherComponents[i]);

            if (comparison != 0)
            {
                return comparison;
            }
        }

        return 0;
    }

    /// <inheritdoc />
    public bool Equals(ValueObject? other)
    {
        if (other == null)
        {
            return false;
        }

        bool areEqual = ValuesAreEqual(other);

        return areEqual;
    }

    /// <inheritdoc />
    public bool Equals(ValueObject? x, ValueObject? y)
    {
        return x == y;
    }

    public int GetHashCode(ValueObject obj)
    {
        return obj._cachedHashCode.GetHashCode();
    }

    /// <summary>
    /// Overrides the default object equality check by comparing types and value-based equality components.
    /// </summary>
    /// <param name="obj">The object to compare with this instance.</param>
    /// <returns>True if the objects are equal, otherwise false.</returns>
    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        // TODO: If ORMs are heavily used and proxy objects are a concern, consider revisiting this.
        // Functionality may need to be added to get the "unproxied" type.
        Type otherType = obj.GetType();
        Type currentType = GetType();

        bool isTypeMismatch = otherType != currentType;

        if (isTypeMismatch)
        {
            return false;
        }

        bool areEqual = ValuesAreEqual((ValueObject)obj);

        return areEqual;
    }

    /// <summary>
    /// Generates a hash code based on the atomic values defining the object.
    /// This method uses deferred execution to compute the hash code only once, caching it for subsequent use,
    /// thus providing a performance benefit in environments where objects are compared frequently.
    /// </summary>
    /// <returns>The computed hash code as an integer.</returns>
    public override int GetHashCode()
    {
        return _cachedHashCode.Value;
    }

    /// <summary>
    /// Returns the atomic values that define the object.
    /// Child classes must implement this method to return all properties that contribute to equality.
    /// Collections and nested complex objects should be immutable and properly implement equality checks to
    /// maintain value object semantics.
    /// </summary>
    /// <returns>An IEnumerable of objects representing the atomic components of this value object.</returns>
    protected abstract IEnumerable<object?> GetEqualityComponents();

    private static int CompareComponents(object? object1, object? object2)
    {
        if (object1 == object2)
        {
            return 0;
        }

        if (object1 is null)
        {
            return -1;
        }

        if (object2 is null)
        {
            return 1;
        }

        if (object1 is IComparable comparable1 && object2 is IComparable comparable2)
        {
            return comparable1.CompareTo(comparable2);
        }

        // Fallback for non-comparable types.
        return string.Compare(object1.GetType().FullName, object2.GetType().FullName, StringComparison.Ordinal);
    }

    private bool ValuesAreEqual(ValueObject valueObject)
    {
        IEnumerable<object?> atomicValues = GetEqualityComponents();
        IEnumerable<object?> otherAtomicValues = valueObject.GetEqualityComponents();
        bool areEqual = atomicValues.SequenceEqual(otherAtomicValues);
        return areEqual;
    }

    private int ComputeHashCode()
    {
        IEnumerable<object?> atomicValues = GetEqualityComponents();

        int result = default;

        // We combine all the hash codes from the individual properties into one integer.

        foreach (object? value in atomicValues)
        {
            int? valueHashCode = value?.GetHashCode() ?? 0;
            result = HashCode.Combine(result, valueHashCode);
        }

        return result;
    }
}
