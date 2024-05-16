using PersonDirectory.Shared.Infrastructure.Abstractios;

namespace PersonDirectory.Shared.Models;

public abstract class Entity : IEntity<Guid>
{
    public DateTimeOffset ChangedAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public Guid Id { get; protected set; }
    public bool Deleted { get; set; }
}
