using PersonDirectory.Shared;
using PersonDirectory.Shared.Models;

namespace PersonDirectory.Domain.CityManagement;

public class City : AggregateRoot<Guid>
{
    public string Name { get; private set; } = string.Empty;

    public static City With(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new AppException(ErrorCodes.InvalidFirstName);

        return new()
        {
            Name = name
        };
    }
}
