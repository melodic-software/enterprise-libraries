using Enterprise.DomainDrivenDesign.Entities;
using Microsoft.EntityFrameworkCore;

// Generic repositories are generally considered to be something to avoid.
// This is here mostly for demonstration purposes.

namespace Enterprise.EntityFramework.Repositories;

// NOTE: A constraint could be added so that this only works with domain entity base abstractions OR aggregate roots.
// The choice depends on if the db context models are being kept separate from the domain models.

// If the TEntityId on EntityBase has a struct constraint, this will need to be updated.
// The class constraint here has been added since we're using record types for IDs

internal abstract class Repository<TEntity, TEntityId> where TEntity : Entity<TEntityId>
    where TEntityId : class, IEquatable<TEntityId>
{
    protected readonly DbContext DbContext;

    protected Repository(DbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<TEntity?> GetByIdAsync(TEntityId id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<TEntity>()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public void Add(TEntity entity)
    {
        DbContext.Add(entity);
    }
}