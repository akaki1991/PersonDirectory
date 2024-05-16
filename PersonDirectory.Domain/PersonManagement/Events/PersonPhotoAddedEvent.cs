using PersonDirectory.Domain.PersonManagement.ValueObjects;
using PersonDirectory.Shared.Models;

namespace PersonDirectory.Domain.PersonManagement.Events;

public class PersonPhotoAddedEvent(Guid PersonId, Photo Photo) : DomainEvent
{
    public Guid PersonId { get; } = PersonId;
    public Photo Photo { get; } = Photo;
}
