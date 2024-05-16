using FluentValidation;
using Microsoft.AspNetCore.Http;
using PersonDirectory.Application.Services;
using PersonDirectory.Application.Shared;
using PersonDirectory.Domain.PersonManagement.Repositories;
using PersonDirectory.Domain.PersonManagement.ValueObjects;
using PersonDirectory.Shared;
using PersonDirectory.Shared.Infrastructure.Abstractios;

namespace PersonDirectory.Application.PersonManagement.Commmands;

public class AddPersonPhotoCommandHandler(IPersonRepository persons, IFileService fileService, IUnitOfWork unitOfWork)
    : ICommandHandler<AddPersonPhotoCommand, AddPersonPhotoCommandResult>
{
    private readonly IPersonRepository _persons = persons;
    private readonly IFileService _fileService = fileService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<AddPersonPhotoCommandResult> Handle(AddPersonPhotoCommand command, CancellationToken cancellationToken)
    {
        var person = await _persons.GetByIdAsync(command.PersonId, cancellationToken)
            ?? throw new AppException(ErrorCodes.PersonNotFound);

        if (person.Photo is not null)
            _fileService.Delete(person.Photo.Url);

        var (FileName, Width, Height) = await _fileService.Upload(command.Photo, cancellationToken);
        person.AddPhoto(new Photo(FileName, Width, Height));

        await _unitOfWork.CommitAsync(cancellationToken);

        return new AddPersonPhotoCommandResult(FileName);
    }
}

public record AddPersonPhotoCommand(Guid PersonId, IFormFile Photo) : ICommand<AddPersonPhotoCommandResult>;
public record AddPersonPhotoCommandResult(string FileName);

public class AddPersonPhotoCommandValidator : AbstractValidator<AddPersonPhotoCommand>
{
    public AddPersonPhotoCommandValidator()
    {
        RuleFor(x => x.Photo.FileName).NotEmpty().WithErrorCode(ErrorCodes.InvalidFileName.ToString());
    }
}