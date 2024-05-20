namespace Enterprise.Library.Core.Extensions;

public static class ListExtensions
{
    /// <summary>
    /// Adds the elements of the specified collection to the end of the <see cref="IList{T}"/>.
    /// </summary>
    /// <param name="list">The list to which elements will be added. Must not be null.</param>
    /// <param name="items">The collection of elements to add to the list. Must not be null.</param>
    /// <exception cref="ArgumentNullException">Thrown if either <paramref name="list"/> or <paramref name="items"/> is null.</exception>
    /// <exception cref="NotSupportedException">Thrown if <paramref name="list"/> is an array, as arrays cannot be resized.</exception>
    /// <remarks>
    /// If <paramref name="list"/> is an instance of <see cref="List{T}"/>, this method uses the more efficient <see cref="List{T}"/>.AddRange method.
    /// If not, it adds items one by one using <see cref="IList{T}"/>.Add method.
    /// This method does not support adding items to arrays as they are fixed size.
    /// </remarks>
    public static void AddRange<T>(this IList<T> list, IEnumerable<T> items)
    {
        ArgumentNullException.ThrowIfNull(list);
        ArgumentNullException.ThrowIfNull(items);

        if (list is List<T> concreteList)
        {
            concreteList.AddRange(items);
        }
        else if (list.GetType().IsArray)
        {
            throw new NotSupportedException($"{nameof(AddRange)} cannot be used on arrays as they are fixed size.");
        }
        else
        {
            foreach (T item in items)
            {
                list.Add(item);
            }
        }
    }
}
