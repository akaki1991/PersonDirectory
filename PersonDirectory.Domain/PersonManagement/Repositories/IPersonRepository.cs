namespace PersonDirectory.Domain.PersonManagement.Repositories;

public interface IPersonRepository
{
    Task AddAsync(Person person, CancellationToken cancellationToken);
    Task<Person?> GetByIdAsync(Guid personId, CancellationToken cancellationToken);
    Task AddPersonRelationshipAsync(PersonRelationship personRelationship, CancellationToken cancellationToken);
    Task<IEnumerable<Person>> GetRelatedPersons(Guid PersonId, CancellationToken cancellationToken);
    Task<PersonRelationship?> GetRealtionShipAsync(Guid targetPersonId, Guid relatedPersonId, CancellationToken cancellationToken);
    void DeletePersonRelationship(PersonRelationship personRelationship);
}
