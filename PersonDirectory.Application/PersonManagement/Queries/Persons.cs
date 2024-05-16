using FluentValidation;
using PersonDirectory.Application.PersonManagement.DTOs;
using PersonDirectory.Application.Shared;
using PersonDirectory.Domain.PersonManagement;
using PersonDirectory.Domain.PersonManagement.ReadServices;
using PersonDirectory.Domain.PersonManagement.ReadServices.DTOs;
using PersonDirectory.Shared;
using PersonDirectory.Shared.Infrastructure.Abstractios;

namespace PersonDirectory.Application.PersonManagement.Queries;

public class PersonsQueryHandler(IPersonReadService readService) : IQueryHandler<PersonsQuery, PersonsQueryResult>
{
    private readonly IPersonReadService _readService = readService;

    public async Task<PersonsQueryResult> Handle(PersonsQuery query, CancellationToken cancellationToken)
    {
        var result = await _readService.SearchPersonsAsync(query.ToSearchModel(), cancellationToken);

        return new PersonsQueryResult(result.Select(x => new PersonsQueryResultItem(
            Id: x.PersonId,
            FirstName: x.FirstName,
            LastName: x.LastName,
            Gender: x.Gender,
            PersonalNumber: x.PersonalNumber,
            DateOfBirth: x.DateOfBirth,
            City: x.City,
            PhoneNumbers: x.PhoneNumbers?.Select(PhoneNumberDTO.From),
            PhotoPath: x.Photo?.Url
            )));
    }
}

public record PersonsQuery(string? FirstName, string? LastName, string? PersonalNumber, string? City,
    DateTime? DateOfBirthStartRange, DateTime? DateOfBirthEndRange, string? PhoneNumber, int Page, int Size)
    : IQuery<PersonsQueryResult>
{
    public PersonDetailsSerachDto ToSearchModel() =>
        new()
        {
            FirstName = FirstName,
            LastName = LastName,
            PersonalNumber = PersonalNumber,
            City = City,
            DateOfBirthStartRange = DateOfBirthStartRange,
            DateOfBirthEndRange = DateOfBirthEndRange,
            PhoneNumber = PhoneNumber,
            Page = Page,
            Size = Size
        };
}

public record PersonsQueryResult(IEnumerable<PersonsQueryResultItem> Persons);
public record PersonsQueryResultItem(Guid Id, string? FirstName, string? LastName, Gender Gender, string? PersonalNumber,
    DateTimeOffset DateOfBirth, string? City, IEnumerable<PhoneNumberDTO>? PhoneNumbers, string? PhotoPath);


public class PersonsQueryValidator : AbstractValidator<PersonsQuery>
{
    public PersonsQueryValidator()
    {
        RuleFor(x => x.Page).NotEmpty().WithErrorCode(ErrorCodes.InvalidPage.ToString());
        RuleFor(x => x.Size).GreaterThan(0).LessThan(30).WithErrorCode(ErrorCodes.InvalidPageSize.ToString());
    }
}
