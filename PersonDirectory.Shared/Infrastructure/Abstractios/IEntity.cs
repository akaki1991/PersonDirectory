namespace PersonDirectory.Shared.Infrastructure.Abstractios;

public interface IEntity<TIdentity>
{
    DateTimeOffset ChangedAt { get; }
    DateTimeOffset CreatedAt { get; }
    TIdentity Id { get; }
    bool Deleted { get; }
}
