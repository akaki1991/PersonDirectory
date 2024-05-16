using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace PersonDirectory.Shared.Infrastructure.Abstractios;

public interface IDbContext
{
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    ChangeTracker ChangeTracker { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
