using PersonDirectory.Shared.Infrastructure.Abstractios;
using System.Collections.ObjectModel;

namespace PersonDirectory.Shared.Models;

public abstract class AggregateRoot<TIdentity> : Entity, IHasDomainEvents where TIdentity : struct
{
    protected AggregateRoot() { }

    public int Version { get; protected set; }
    protected IList<DomainEvent> Events { get; } = new List<DomainEvent>();

    public void MarkChangesAsCommitted() => Events.Clear();

    public IReadOnlyList<DomainEvent> UncommittedChanges() => new ReadOnlyCollection<DomainEvent>(Events);

    protected void Raise(DomainEvent @event)
    {
        @event.EventId = Guid.NewGuid();
        @event.OccuredOn = DateTimeOffset.Now;
        @event.Type = @event.GetType().Name;

        if (Version == default)
            CreatedAt = DateTimeOffset.Now;

        Version++;
        ChangedAt = DateTimeOffset.Now;
        Events.Add(@event);
    }
}
