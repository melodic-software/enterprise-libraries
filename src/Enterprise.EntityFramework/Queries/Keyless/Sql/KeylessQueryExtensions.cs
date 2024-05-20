using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Enterprise.EntityFramework.Queries.Keyless.Sql;

public static class KeylessQueryExtensions
{
    [SuppressMessage("Usage", "S6966:Await QueryAsync instead", Justification = "The analyzer is incorrectly flagging this method.")]
    public static async Task<List<T>> QueryAsync<T>(this DatabaseFacade dbFacade, FormattableString sqlQuery)
    {
        return await dbFacade.Query<T>(sqlQuery).ToListAsync();
    }

    public static IQueryable<T> Query<T>(this DbContext dbContext, FormattableString sqlQuery)
    {
        return dbContext.Database.Query<T>(sqlQuery);
    }

    public static IQueryable<T> Query<T>(this DatabaseFacade dbFacade, FormattableString sqlQuery)
    {
        IQueryable<T> query = dbFacade
            .SqlQuery<T>(sqlQuery);

        return query;
    }
}
