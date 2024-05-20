namespace Enterprise.Library.Extensions;

public static class ListExtensions
{
    public static void AddRange<T>(this IList<T> list, IEnumerable<T> items)
    {
        ArgumentNullException.ThrowIfNull(list);
        ArgumentNullException.ThrowIfNull(items);

        if (list is List<T> concreteList)
        {
            concreteList.AddRange(items);
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
