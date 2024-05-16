using Microsoft.EntityFrameworkCore;
using PersonDirectory.Domain.CityManagement;
using PersonDirectory.Domain.CityManagement.Repositories;
using PersonDirectory.Infrastructure.DataAccess;

namespace PersonDirectory.Infrastructure.Repositories;

public class CityRepository(PersonDirectoryDbContext db) : ICityRepository
{
    private readonly PersonDirectoryDbContext _db = db;

    public async Task AddAsync(City city, CancellationToken cancellationToken) =>
        await _db.Cities.AddAsync(city, cancellationToken);

    public async Task<City?> GetByIdAsync(Guid cityId, CancellationToken cancellationToken) =>
        await _db.Cities.FirstOrDefaultAsync(x => x.Id == cityId, cancellationToken);

    public async Task<IEnumerable<City>> GetCities(CancellationToken cancellationToken) =>
        await _db.Cities.ToArrayAsync(cancellationToken: cancellationToken);

    public void Remove(City city) => _db.Cities.Remove(city);
}
