using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Enterprise.EntityFramework.Decorators;

public class TransactionalDecorator
{
    public void Execute(DbContext context, Action execute)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(execute);

        IDbContextTransaction? transaction = null;

        try
        {
            transaction = context.Database.BeginTransaction();
            execute.Invoke();
            transaction.Commit(); // If this commit line isn't reached, EF core will auto rollback.
        }
        catch (DbUpdateConcurrencyException)
        {
            // Apply logic for handling concurrency exceptions.
            // https://learn.microsoft.com/en-us/ef/core/saving/concurrency
        }
        catch (Exception)
        {
            // This technically doesn't need to be called manually (see comment above),
            // but it doesn't hurt to leave it in.
            transaction?.Rollback();
        }
        finally
        {
            transaction?.Dispose();
        }
    }
}