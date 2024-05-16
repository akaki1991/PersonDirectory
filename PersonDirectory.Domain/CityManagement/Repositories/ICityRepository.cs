namespace PersonDirectory.Domain.CityManagement.Repositories;

public interface ICityRepository
{
    Task AddAsync(City city, CancellationToken cancellationToken);
    Task<City?> GetByIdAsync(Guid personId, CancellationToken cancellationToken);
    Task<IEnumerable<City>> GetCities(CancellationToken cancellationToken);
    void Remove(City city);
}
