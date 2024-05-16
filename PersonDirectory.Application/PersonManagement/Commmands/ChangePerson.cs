using FluentValidation;
using Newtonsoft.Json;
using PersonDirectory.Application.PersonManagement.DTOs;
using PersonDirectory.Application.PersonManagement.Shared.Helpers;
using PersonDirectory.Application.Shared;
using PersonDirectory.Domain.CityManagement.Repositories;
using PersonDirectory.Domain.PersonManagement;
using PersonDirectory.Domain.PersonManagement.Repositories;
using PersonDirectory.Domain.PersonManagement.ValueObjects;
using PersonDirectory.Shared;
using PersonDirectory.Shared.Infrastructure.Abstractios;
using Swashbuckle.AspNetCore.Annotations;

namespace PersonDirectory.Application.PersonManagement.Commmands;

public class ChangePersonCommandHandler(IPersonRepository persons, ICityRepository cities, IUnitOfWork unitOfWork)
    : ICommandHandler<ChangePersonCommand, ChangePersonCommandResult>
{
    private readonly IPersonRepository _persons = persons;
    private readonly ICityRepository _cities = cities;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ChangePersonCommandResult> Handle(ChangePersonCommand command, CancellationToken cancellationToken)
    {
        var person = await _persons.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new AppException(ErrorCodes.PersonNotFound);

        var city = await _cities.GetByIdAsync(command.CityId, cancellationToken)
            ?? throw new AppException(ErrorCodes.CityNotFound);

        person.ChangeDetails(
            firstName: command.FirstName,
            lastName: command.LastName,
            gender: command.Gender,
            personalNumber: command.PersonalNumber,
            dateOfBirth: command.DateOfBirth,
            address: new Address(city.Name),
            phoneNumbers: command.PhoneNumbers?.Select(x => x.ToDomainModel())
            );

        await _unitOfWork.CommitAsync(cancellationToken);

        return new ChangePersonCommandResult(person.Id);
    }
}

public record ChangePersonCommand(string FirstName, string LastName, Gender Gender, string PersonalNumber,
    DateTime DateOfBirth, Guid CityId, IEnumerable<PhoneNumberDTO>? PhoneNumbers) : ICommand<ChangePersonCommandResult>
{
    [JsonIgnore]
    [SwaggerSchema(ReadOnly = true)]
    public Guid Id { get; init; }
}

public record ChangePersonCommandResult(Guid Id);

public class ChangePersonCommandValidator : AbstractValidator<ChangePersonCommand>
{
    public ChangePersonCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().Length(2, 50).SetValidator(new GeorgianLatinLettersValidation<ChangePersonCommand, string>()).WithErrorCode(ErrorCodes.InvalidFirstName.ToString());
        RuleFor(x => x.LastName).NotEmpty().Length(2, 50).SetValidator(new GeorgianLatinLettersValidation<ChangePersonCommand, string>()).WithErrorCode(ErrorCodes.InvalidLastName.ToString());
        RuleFor(x => x.Gender).NotEqual(Gender.None).WithErrorCode(ErrorCodes.InvalidGender.ToString());
        RuleFor(x => x.PersonalNumber).NotEmpty().Length(11).Must(x => x.All(char.IsDigit)).WithErrorCode(ErrorCodes.InvalidPersonalNumber.ToString());
        RuleFor(x => x.DateOfBirth).LessThanOrEqualTo(DateTime.Now.AddYears(-18)).WithErrorCode(ErrorCodes.InvalidAge.ToString());
        RuleFor(x => x.PhoneNumbers).Must(DataValidationHelpers.IsValidPhoneNumbers).WithErrorCode(ErrorCodes.InvalidPhoneNumber.ToString());
    }
}
