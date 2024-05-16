using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PersonDirectory.Domain.PersonManagement.ReadModels;
using PersonDirectory.Domain.PersonManagement.ReadServices;
using PersonDirectory.Domain.PersonManagement.ReadServices.DTOs;
using PersonDirectory.Infrastructure.DataAccess;

namespace PersonDirectory.Infrastructure.ReadServices;

public class PersonReadService(PersonDirectoryDbContext db) : IPersonReadService
{
    private readonly PersonDirectoryDbContext _db = db;

    public async Task<PersonReadModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        await _db.PersonReadModels.FindAsync(id, cancellationToken);

    public async Task<IEnumerable<PersonReadModel>> SearchPersonsAsync(PersonDetailsSerachDto searchModel, CancellationToken cancellationToken)
    {
        var persons = _db.PersonReadModels.AsQueryable();

        if (searchModel is not null)
        {
            if (!string.IsNullOrEmpty(searchModel.FirstName))
                persons = persons.Where(x => x.FirstName!.Contains(searchModel.FirstName));

            if (!string.IsNullOrEmpty(searchModel.LastName))
                persons = persons.Where(x => x.LastName!.Contains(searchModel.LastName));

            if (!string.IsNullOrEmpty(searchModel.PersonalNumber))
                persons = persons.Where(x => x.PersonalNumber!.Contains(searchModel.PersonalNumber));

            if (!string.IsNullOrEmpty(searchModel.City))
                persons = persons.Where(x => x.City!.Contains(searchModel.City));

            if (searchModel.DateOfBirthStartRange.HasValue || searchModel.DateOfBirthEndRange.HasValue)
                persons = persons.Where(pp => (!searchModel.DateOfBirthStartRange.HasValue || pp.DateOfBirth >= searchModel.DateOfBirthStartRange.Value)
                && (!searchModel.DateOfBirthEndRange.HasValue || pp.DateOfBirth <= searchModel.DateOfBirthEndRange.Value));

            if (!string.IsNullOrEmpty(searchModel.PhoneNumber))
            {
                var phoneNumber = searchModel.PhoneNumber;
                var query = @$"
                SELECT Id 
                FROM ReadModels.PersonReadModels 
                WHERE JSON_VALUE(PhoneNumbers, '$[0].Number') LIKE @phoneNumber
                OR JSON_VALUE(PhoneNumbers, '$[1].Number') LIKE @phoneNumber
                OR JSON_VALUE(PhoneNumbers, '$[2].Number') LIKE @phoneNumber";

                var matchingIds = await _db.PersonReadModels
                    .FromSqlRaw(query, new SqlParameter("@phoneNumber", $"%{phoneNumber}%"))
                    .Select(pr => pr.Id)
                    .ToListAsync(cancellationToken);

                persons = persons.Where(p => matchingIds.Contains(p.Id));
            }
        }

        var skip = searchModel?.Page is null || searchModel?.Page is 0 ? 0 : (searchModel.Page - 1) * searchModel.Size;
        var take = searchModel?.Size is null || searchModel?.Size is 0 ? 20 : searchModel.Size;

        return await persons.Skip(skip).Take(take).ToArrayAsync(cancellationToken);
    }
}
