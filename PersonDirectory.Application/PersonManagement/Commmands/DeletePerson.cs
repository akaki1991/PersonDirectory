using FluentValidation;
using PersonDirectory.Application.Services;
using PersonDirectory.Application.Shared;
using PersonDirectory.Domain.PersonManagement.Repositories;
using PersonDirectory.Shared;
using PersonDirectory.Shared.Infrastructure.Abstractios;

namespace PersonDirectory.Application.PersonManagement.Commmands;

public class DeletePersonCommandHandler(IPersonRepository persons, IFileService fileService, IUnitOfWork unitOfWork) : ICommandHandler<DeletePersonCommand, DeletePersonCommandResult>
{
    private readonly IPersonRepository _persons = persons;
    private readonly IFileService _fileService = fileService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<DeletePersonCommandResult> Handle(DeletePersonCommand command, CancellationToken cancellationToken)
    {
        var person = await _persons.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new AppException(ErrorCodes.PersonNotFound);

        person.MarkAsDeleted();

        if (person.Photo is not null)
            _fileService.Delete(person.Photo.Url);

        await _unitOfWork.CommitAsync(cancellationToken);

        return new DeletePersonCommandResult();
    }
}

public record DeletePersonCommand(Guid Id) : ICommand<DeletePersonCommandResult>;
public record DeletePersonCommandResult;

public class DeletePersonCommandValidation : AbstractValidator<DeletePersonCommand>
{
    public DeletePersonCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty().WithErrorCode(ErrorCodes.InvalidId.ToString());
    }
}
