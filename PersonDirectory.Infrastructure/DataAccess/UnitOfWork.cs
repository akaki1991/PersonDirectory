using PersonDirectory.Shared.Infrastructure.Abstractios;

namespace PersonDirectory.Infrastructure.DataAccess;

public class UnitOfWork(PersonDirectoryDbContext db, IDomainEventsDispatcher domainEventsDispatcher) : IUnitOfWork
{
    private readonly IDomainEventsDispatcher _domainEventsDispatcher = domainEventsDispatcher;
    private readonly PersonDirectoryDbContext _db = db;

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        await _domainEventsDispatcher.DispatchEventsAsync();

        return await _db.SaveChangesAsync(cancellationToken);
    }
}
