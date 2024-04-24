using Microsoft.Extensions.Logging;

namespace Enterprise.Logging.Core.Events;

public static class LogEventIds
{
    // EventIds are not required, but use if it helps.
    // Consider using "ranges" to isolate feature entries.
    // This implies some forethought / organization with numbering.
    // Ex: 1000-1999 = "browsing products" feature.
    // Ex: 2000-2999 = "checking out" feature.
    // This enables another way to query and filter log entries.

    public static EventId UnknownError = new(0, "UnknownError");
    public static EventId CustomError = new(10, "CustomError");
}