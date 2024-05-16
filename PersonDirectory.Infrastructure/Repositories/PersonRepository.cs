using Microsoft.EntityFrameworkCore;
using PersonDirectory.Domain.PersonManagement;
using PersonDirectory.Domain.PersonManagement.Repositories;
using PersonDirectory.Infrastructure.DataAccess;

namespace PersonDirectory.Infrastructure.Repositories;

public class PersonRepository(PersonDirectoryDbContext db) : IPersonRepository
{
    private readonly PersonDirectoryDbContext _db = db;

    public async Task AddAsync(Person person, CancellationToken cancellationToken) =>
        await _db.Persons.AddAsync(person, cancellationToken);

    public async Task<Person?> GetByIdAsync(Guid personId, CancellationToken cancellationToken) =>
        await _db.Persons.FirstOrDefaultAsync(x => x.Id == personId, cancellationToken);

    public async Task AddPersonRelationshipAsync(PersonRelationship personRelationship, CancellationToken cancellationToken) =>
        await _db.PersonRelationships.AddAsync(personRelationship, cancellationToken);

    public async Task<IEnumerable<Person>> GetRelatedPersons(Guid PersonId, CancellationToken cancellationToken)
    {
        var relatedPersonIds = await _db.PersonRelationships.Where(x => x.TargetPersonId == PersonId)
            .Select(x => x.RelatedPersonId)
            .ToArrayAsync(cancellationToken);

        if (relatedPersonIds is null || relatedPersonIds.Length is 0)
            return Enumerable.Empty<Person>();

        return await _db.Persons.Where(p => relatedPersonIds.Contains(p.Id)).ToArrayAsync(cancellationToken);
    }

    public async Task<PersonRelationship?> GetRealtionShipAsync(Guid targetPersonId, Guid relatedPersonId, CancellationToken cancellationToken) =>
        await _db.PersonRelationships.FirstOrDefaultAsync(x => x.TargetPersonId == targetPersonId
                                            && x.RelatedPersonId == relatedPersonId, cancellationToken);

    public void DeletePersonRelationship(PersonRelationship personRelationship) =>
        _db.PersonRelationships.Remove(personRelationship);
}
