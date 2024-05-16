using PersonDirectory.Shared.Infrastructure.Abstractios;

namespace PersonDirectory.Shared.Models;

public abstract class DomainEvent : Event, IDomainEvent
{
    public DateTimeOffset OccuredOn { get; set; }
    public string? Type { get; set; }
}