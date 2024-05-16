using PersonDirectory.Shared.Models;

namespace PersonDirectory.Shared.Infrastructure.Abstractios;

public interface IHasDomainEvents
{
    IReadOnlyList<DomainEvent> UncommittedChanges();
    void MarkChangesAsCommitted();
}
