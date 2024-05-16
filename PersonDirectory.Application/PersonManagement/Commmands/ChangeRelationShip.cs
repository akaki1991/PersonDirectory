using FluentValidation;
using Newtonsoft.Json;
using PersonDirectory.Application.Shared;
using PersonDirectory.Domain.PersonManagement;
using PersonDirectory.Domain.PersonManagement.Repositories;
using PersonDirectory.Shared;
using PersonDirectory.Shared.Infrastructure.Abstractios;
using Swashbuckle.AspNetCore.Annotations;

namespace PersonDirectory.Application.PersonManagement.Commmands;

public class ChangeRelationShipCommandHandler(IPersonRepository persons, IUnitOfWork unitOfWork) 
    : ICommandHandler<ChangeRelationShipCommand, ChangeRelationShipCommandReuslt>
{
    private readonly IPersonRepository _persons = persons;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ChangeRelationShipCommandReuslt> Handle(ChangeRelationShipCommand command, CancellationToken cancellationToken)
    {
        var personRelationShip = await _persons.GetRealtionShipAsync(targetPersonId: command.PersonId, 
            relatedPersonId: command.RelatedPersonId, cancellationToken) 
            ?? throw new AppException(ErrorCodes.RelationshipDoesNotExist);

        personRelationShip.ChangeRelationshipType(command.PersonRelationshipType);

        await _unitOfWork.CommitAsync(cancellationToken);
        return new ChangeRelationShipCommandReuslt(command.PersonId);
    }
}

public record ChangeRelationShipCommand(PersonRelationshipType PersonRelationshipType)
    : ICommand<ChangeRelationShipCommandReuslt>
{
    [JsonIgnore]
    [SwaggerSchema(ReadOnly = true)]
    public Guid PersonId { get; init; }

    [JsonIgnore]
    [SwaggerSchema(ReadOnly = true)]
    public Guid RelatedPersonId { get; init; }
}
public record ChangeRelationShipCommandReuslt(Guid PersonId);

public class ChangeRelationShipCommandValidator : AbstractValidator<ChangeRelationShipCommand>
{
    public ChangeRelationShipCommandValidator()
    {
        RuleFor(x => x.PersonRelationshipType).NotEmpty().WithErrorCode(ErrorCodes.InvalidRelationshipType.ToString());
        RuleFor(x => x.RelatedPersonId).NotEmpty().WithErrorCode(ErrorCodes.InvalidRelatedPersonId.ToString());
        RuleFor(x => x.PersonId).NotEmpty().WithErrorCode(ErrorCodes.InvalidPersonId.ToString());
    }
}