using FluentValidation;
using Newtonsoft.Json;
using PersonDirectory.Application.Shared;
using PersonDirectory.Domain.PersonManagement;
using PersonDirectory.Domain.PersonManagement.Repositories;
using PersonDirectory.Shared;
using PersonDirectory.Shared.Infrastructure.Abstractios;
using Swashbuckle.AspNetCore.Annotations;

namespace PersonDirectory.Application.PersonManagement.Commmands;

public class AddRelatedPersonCommmandHandler(IPersonRepository persons, IUnitOfWork unitOfWork)
    : ICommandHandler<AddRelatedPersonCommmand, AddRelatedPersonCommmandResult>
{
    private readonly IPersonRepository _persons = persons;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<AddRelatedPersonCommmandResult> Handle(AddRelatedPersonCommmand command, CancellationToken cancellationToken)
    {
        var person = await _persons.GetByIdAsync(command.PersonId, cancellationToken)
            ?? throw new AppException(ErrorCodes.PersonNotFound);

        var relatedPerson = await _persons.GetByIdAsync(command.ReletedPersonId, cancellationToken)
            ?? throw new AppException(ErrorCodes.RelatedPersonNotFound);

        var personRealation = new PersonRelationship(person.Id, relatedPerson.Id, command.PersonRelationshipType);

        await _persons.AddPersonRelationshipAsync(personRealation, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

        return new AddRelatedPersonCommmandResult(person.Id);
    }
}

public record AddRelatedPersonCommmand(Guid ReletedPersonId, PersonRelationshipType PersonRelationshipType) : ICommand<AddRelatedPersonCommmandResult>
{
    [JsonIgnore]
    [SwaggerSchema(ReadOnly = true)]
    public Guid PersonId { get; init; }
}

public record AddRelatedPersonCommmandResult(Guid PersonId);

public class AddRelatedPersonCommmandValidator : AbstractValidator<AddRelatedPersonCommmand>
{
    public AddRelatedPersonCommmandValidator()
    {
        RuleFor(x => x.PersonId).NotEmpty().WithErrorCode(ErrorCodes.InvalidPersonId.ToString());
        RuleFor(x => x.ReletedPersonId).NotEmpty().WithErrorCode(ErrorCodes.InvalidRelatedPersonId.ToString());
        RuleFor(x => x.PersonRelationshipType).IsInEnum().WithErrorCode(ErrorCodes.RelationshipDoesNotExist.ToString());
        RuleFor(x => x.PersonId).NotEqual(x => x.ReletedPersonId).WithErrorCode(ErrorCodes.PersonAndRelatedPersonAreSame.ToString());
    }
}