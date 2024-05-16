using PersonDirectory.Domain.CityManagement.Repositories;
using PersonDirectory.Shared.Infrastructure.Abstractios;

namespace PersonDirectory.Application.CityManagement.Queries;

public class CitiesQueryHandler(ICityRepository cities) : IQueryHandler<CitiesQuery, CitiesQueryResult>
{
    private readonly ICityRepository _cities = cities;

    public async Task<CitiesQueryResult> Handle(CitiesQuery request, CancellationToken cancellationToken)
    {
        var cities = await _cities.GetCities(cancellationToken);

        return new CitiesQueryResult(cities.Select(c => new CitiesQueryResultItem(c.Id, c.Name)));
    }
}

public record CitiesQuery : IQuery<CitiesQueryResult>;

public record CitiesQueryResult(IEnumerable<CitiesQueryResultItem> Cities);

public record CitiesQueryResultItem(Guid Id, string Name);

