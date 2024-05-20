using Enterprise.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Enterprise.EntityFramework.AspNetCore.Concurrency;

public class ConcurrencyErrorHandlingInterceptor : SaveChangesInterceptor
{
    public override void SaveChangesFailed(DbContextErrorEventData eventData)
    {
        // Wrap the EF exception in a common enterprise concurrency exception.
        if (eventData.Exception is DbUpdateConcurrencyException concurrencyException)
        {
            throw new ConcurrencyException("Concurrency exception occurred.", concurrencyException);
        }

        base.SaveChangesFailed(eventData);
    }

    public override Task SaveChangesFailedAsync(DbContextErrorEventData eventData, CancellationToken cancellationToken = default)
    {
        if (eventData.Exception is DbUpdateConcurrencyException concurrencyException)
        {
            throw new ConcurrencyException("Concurrency exception occurred.", concurrencyException);
        }

        return base.SaveChangesFailedAsync(eventData, cancellationToken);
    }
}
