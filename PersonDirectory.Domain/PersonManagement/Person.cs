using PersonDirectory.Domain.PersonManagement.Events;
using PersonDirectory.Domain.PersonManagement.ValueObjects;
using PersonDirectory.Shared.Models;

namespace PersonDirectory.Domain.PersonManagement;

public class Person : AggregateRoot<Guid>
{
    public Person() { }

    public Person(string firstName,
        string lastName,
        Gender gender,
        string personalNumber,
        DateTime dateOfBirth,
        Address address,
        IEnumerable<PhoneNumber>? phoneNumbers)
    {
        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Gender = gender;
        PersonalNumber = personalNumber;
        DateOfBirth = dateOfBirth;
        Address = address;
        PhoneNumbers = phoneNumbers;

        Raise(new PersonCreatedEvent(this));
    }

    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public Gender Gender { get; private set; }
    public string? PersonalNumber { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public Address? Address { get; private set; }
    public IEnumerable<PhoneNumber>? PhoneNumbers { get; private set; }
    public Photo? Photo { get; private set; }

    public void ChangeDetails(string firstName,
        string lastName,
        Gender gender,
        string personalNumber,
        DateTime dateOfBirth,
        Address address,
        IEnumerable<PhoneNumber>? phoneNumbers)
    {
        FirstName = firstName;
        LastName = lastName;
        Gender = gender;
        PersonalNumber = personalNumber;
        DateOfBirth = dateOfBirth;
        Address = address;
        PhoneNumbers = phoneNumbers;

        Raise(new PersonChangedEvent(this));
    }

    public void AddPhoto(Photo photo)
    {
        Photo = photo;

        Raise(new PersonPhotoAddedEvent(Id, photo));
    }

    public void MarkAsDeleted()
    {
        Deleted = true;

        Raise(new PersonDeletedEvent(Id));
    }
}
