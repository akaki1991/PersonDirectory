using Microsoft.EntityFrameworkCore;
using PersonDirectory.Domain.PersonManagement;
using PersonDirectory.Infrastructure.DataAccess;
using PersonDirectory.Shared.Infrastructure.Abstractios;

namespace PersonDirectory.Application.PersonManagement.Queries;

public class RelationsReportQueryHandler(PersonDirectoryDbContext db) : IQueryHandler<RelationsReportQuery, RelationsReportQueryResult>
{
    private readonly PersonDirectoryDbContext _db = db;

    public async Task<RelationsReportQueryResult> Handle(RelationsReportQuery request, CancellationToken cancellationToken)
    {
        var result = await _db.PersonRelationships
            .AsNoTracking()
            .GroupBy(rpp => rpp.TargetPersonId)
            .Select(group => new Person(
                group.Key,
                group.GroupBy(rpp => rpp.PersonRelationshipType)
                     .Select(subGroup => new RelatedPerson(
                         subGroup.Key,
                         subGroup.Count()
                     ))
            ))
            .ToListAsync(cancellationToken);

        return new RelationsReportQueryResult(result);
    }
}

public record RelationsReportQuery: IQuery<RelationsReportQueryResult>;

public record RelationsReportQueryResult(IEnumerable<Person> Persons);

public record Person(Guid PersonId, IEnumerable<RelatedPerson> RelatedPersons);

public record RelatedPerson(PersonRelationshipType PersonRelationshipType, int RelatedPersonsCount);
