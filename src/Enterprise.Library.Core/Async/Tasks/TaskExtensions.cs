namespace Enterprise.Library.Core.Async.Tasks;

public static class TaskExtensions
{
    public static void WaitAll(this IEnumerable<Task> tasks) => Task.WaitAll(tasks.ToArray());
    public static void WaitAll(this IEnumerable<Task> tasks, TimeSpan timeout) => Task.WaitAll(tasks.ToArray(), timeout);
    public static void WaitAll(this IEnumerable<Task> tasks, int timeoutInMilliseconds) => Task.WaitAll(tasks.ToArray(), timeoutInMilliseconds);
    public static void WaitAll(this IEnumerable<Task> tasks, int timeoutInMilliseconds, CancellationToken cancellationToken) => Task.WaitAll(tasks.ToArray(), timeoutInMilliseconds, cancellationToken);
    public static void WaitAll(this IEnumerable<Task> tasks, CancellationToken cancellationToken) => Task.WaitAll(tasks.ToArray(), cancellationToken);
    public static async Task WhenAll(this IEnumerable<Task> tasks) => await Task.WhenAll(tasks.ToArray());
    public static async Task WhenAny(this IEnumerable<Task> tasks) => await Task.WhenAny(tasks.ToArray());
}