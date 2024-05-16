using PersonDirectory.Shared.Models;

namespace PersonDirectory.Domain.PersonManagement.Events;

public class PersonDeletedEvent(Guid aggregateRootId) : DomainEvent
{
    public Guid AggregateRootId { get; } = aggregateRootId;
}
