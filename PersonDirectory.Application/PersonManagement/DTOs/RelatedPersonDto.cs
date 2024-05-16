using PersonDirectory.Domain.PersonManagement;

namespace PersonDirectory.Application.PersonManagement.DTOs;

public class RelatedPersonDto
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public static RelatedPersonDto From(Person person) =>
        new()
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
        };
}
