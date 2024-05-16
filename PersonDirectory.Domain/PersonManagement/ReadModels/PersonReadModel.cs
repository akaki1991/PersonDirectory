using PersonDirectory.Domain.PersonManagement.Events;
using PersonDirectory.Domain.PersonManagement.ValueObjects;

namespace PersonDirectory.Domain.PersonManagement.ReadModels;

public class PersonReadModel
{
    public Guid Id { get; set; }
    public Guid PersonId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public Gender Gender { get; set; }
    public string? PersonalNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? City { get; set; }
    public IEnumerable<PhoneNumber>? PhoneNumbers { get; set; }
    public Photo? Photo { get; set; }

    public static PersonReadModel From(PersonCreatedEvent @event) =>
        new()
        {
            FirstName = @event.FirstName,
            LastName = @event.LastName,
            Gender = @event.Gender,
            PersonalNumber = @event.PersonalNumber,
            DateOfBirth = @event.DateOfBirth,
            City = @event.Address?.City,
            PhoneNumbers = @event.PhoneNumbers,
            PersonId = @event.AggregateRootId,
            Photo = @event.Photo
        };
}
