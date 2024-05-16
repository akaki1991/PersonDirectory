using FluentValidation;
using PersonDirectory.Application.PersonManagement.DTOs;
using PersonDirectory.Application.PersonManagement.Shared.Helpers;
using PersonDirectory.Application.Shared;
using PersonDirectory.Domain.CityManagement.Repositories;
using PersonDirectory.Domain.PersonManagement;
using PersonDirectory.Domain.PersonManagement.Repositories;
using PersonDirectory.Domain.PersonManagement.ValueObjects;
using PersonDirectory.Shared;
using PersonDirectory.Shared.Infrastructure.Abstractios;

namespace PersonDirectory.Application.PersonManagement.Commmands;

public class AddPersonCommandHandler(IPersonRepository persons, ICityRepository cities, IUnitOfWork unitOfWork)
    : ICommandHandler<AddPersonCommand, AddPersonCommandResult>
{
    private readonly IPersonRepository _persons = persons;
    private readonly ICityRepository _cities = cities;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<AddPersonCommandResult> Handle(AddPersonCommand command, CancellationToken cancellationToken)
    {
        var city = await _cities.GetByIdAsync(command.CityId, cancellationToken)
            ?? throw new AppException(ErrorCodes.CityNotFound);

        var person = new Person(
            firstName: command.FirstName,
            lastName: command.LastName,
            gender: command.Gender,
            personalNumber: command.PersonalNumber,
            dateOfBirth: command.DateOfBirth,
            address: new Address(city.Name),
            phoneNumbers: command.PhoneNumbers?.Select(p => p.ToDomainModel())
            );

        await _persons.AddAsync(person, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

        return new AddPersonCommandResult(person.Id);
    }
}

public record AddPersonCommand(string FirstName, string LastName, Gender Gender, string PersonalNumber,
    DateTime DateOfBirth, Guid CityId, IEnumerable<PhoneNumberDTO>? PhoneNumbers) : ICommand<AddPersonCommandResult>;

public record AddPersonCommandResult(Guid Id);

public class AddPersonCommandValidator : AbstractValidator<AddPersonCommand>
{
    public AddPersonCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().Length(2, 50).SetValidator(new GeorgianLatinLettersValidation<AddPersonCommand, string>()).WithErrorCode(ErrorCodes.InvalidFirstName.ToString());
        RuleFor(x => x.LastName).NotEmpty().Length(2, 50).SetValidator(new GeorgianLatinLettersValidation<AddPersonCommand, string>()).WithErrorCode(ErrorCodes.InvalidLastName.ToString());
        RuleFor(x => x.Gender).NotEqual(Gender.None).WithErrorCode(ErrorCodes.InvalidGender.ToString());
        RuleFor(x => x.PersonalNumber).NotEmpty().Length(11).Must(x => x.All(char.IsDigit)).WithErrorCode(ErrorCodes.InvalidPersonalNumber.ToString());
        RuleFor(x => x.DateOfBirth).LessThanOrEqualTo(DateTime.Now.AddYears(-18)).WithErrorCode(ErrorCodes.InvalidAge.ToString());
        RuleFor(x => x.PhoneNumbers).Must(DataValidationHelpers.IsValidPhoneNumbers).WithErrorCode(ErrorCodes.InvalidPhoneNumber.ToString());
    }
}