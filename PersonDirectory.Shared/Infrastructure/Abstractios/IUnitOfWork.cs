namespace PersonDirectory.Shared.Infrastructure.Abstractios;

public interface IUnitOfWork
{
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}
