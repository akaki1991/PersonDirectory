using PersonDirectory.Domain.PersonManagement.ValueObjects;
using PersonDirectory.Shared.Models;

namespace PersonDirectory.Domain.PersonManagement.Events;

public class PersonChangedEvent(Person person) : DomainEvent
{
    public Guid AggregateRootId { get; } = person.Id;
    public string? FirstName { get; } = person.FirstName;
    public string? LastName { get; } = person.LastName;
    public Gender Gender { get; } = person.Gender;
    public string? PersonalNumber { get; } = person.PersonalNumber;
    public DateTime DateOfBirth { get; } = person.DateOfBirth;
    public Address? Address { get; } = person.Address;
    public IEnumerable<PhoneNumber>? PhoneNumbers { get; } = person.PhoneNumbers;
    public Photo? Photo { get; } = person.Photo;
}
