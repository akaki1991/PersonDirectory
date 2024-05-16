using PersonDirectory.Application.Shared;
using PersonDirectory.Domain.PersonManagement.Repositories;
using PersonDirectory.Shared;
using PersonDirectory.Shared.Infrastructure.Abstractios;

namespace PersonDirectory.Application.PersonManagement.Commmands;

public class DeleteRelationshipCommandHanlder(IPersonRepository persons, IUnitOfWork unitOfWork) : ICommandHandler<DeleteRelationshipCommand, DeleteRelationshipCommandResult>
{
    private readonly IPersonRepository _persons = persons;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<DeleteRelationshipCommandResult> Handle(DeleteRelationshipCommand command, CancellationToken cancellationToken)
    {
        var personRelationShip = await _persons.GetRealtionShipAsync(targetPersonId: command.PersonId,
            relatedPersonId: command.RelatedPersonId, cancellationToken)
            ?? throw new AppException(ErrorCodes.RelationshipDoesNotExist);

        _persons.DeletePersonRelationship(personRelationShip);

        await _unitOfWork.CommitAsync(cancellationToken);

        return new DeleteRelationshipCommandResult();
    }
}

public record DeleteRelationshipCommand(Guid PersonId, Guid RelatedPersonId) : ICommand<DeleteRelationshipCommandResult>;

public record DeleteRelationshipCommandResult;
