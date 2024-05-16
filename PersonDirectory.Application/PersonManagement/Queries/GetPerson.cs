using FluentValidation;
using PersonDirectory.Application.PersonManagement.DTOs;
using PersonDirectory.Application.Shared;
using PersonDirectory.Domain.PersonManagement;
using PersonDirectory.Domain.PersonManagement.ReadServices;
using PersonDirectory.Domain.PersonManagement.Repositories;
using PersonDirectory.Shared;
using PersonDirectory.Shared.Infrastructure.Abstractios;

namespace PersonDirectory.Application.PersonManagement.Queries;

public class GetPersonQueryHandler(IPersonReadService readService, IPersonRepository persons) : IQueryHandler<GetPersonQuery, GetPersonQueryResult>
{
    private readonly IPersonReadService _readService = readService;
    private readonly IPersonRepository _persons = persons;

    public async Task<GetPersonQueryResult> Handle(GetPersonQuery query, CancellationToken cancellationToken)
    {
        var person = await _readService.GetByIdAsync(query.Id, cancellationToken)
            ?? throw new AppException(ErrorCodes.PersonNotFound);

        var relatedPersons = await _persons.GetRelatedPersons(person.Id, cancellationToken);

        return new GetPersonQueryResult(Id: person.Id,
            FirstName: person.FirstName,
            LastName: person.LastName,
            Gender: person.Gender,
            PersonalNumber: person.PersonalNumber,
            DateOfBirth: person.DateOfBirth,
            City: person.City,
            Phones: person.PhoneNumbers?.Select(PhoneNumberDTO.From),
            PhotoPath: person.Photo?.Url,
            RelatedPersons: relatedPersons.Select(RelatedPersonDto.From));
    }
}

public record GetPersonQuery(Guid Id) : IQuery<GetPersonQueryResult>;
public record GetPersonQueryResult(Guid Id, string? FirstName, string? LastName, Gender Gender, string? PersonalNumber,
    DateTimeOffset DateOfBirth, string? City, IEnumerable<PhoneNumberDTO>? Phones, string? PhotoPath,
    IEnumerable<RelatedPersonDto> RelatedPersons);

public class GetPersonQueryValidator : AbstractValidator<GetPersonQuery>
{
    public GetPersonQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithErrorCode(ErrorCodes.InvalidId.ToString());
    }
}


