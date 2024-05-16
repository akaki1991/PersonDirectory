using FluentValidation;
using PersonDirectory.Domain.CityManagement.Repositories;
using PersonDirectory.Shared;
using PersonDirectory.Shared.Infrastructure.Abstractios;

namespace PersonDirectory.Application.CityManagement.Commands;

public class DeleteCityCommandHanlder(ICityRepository cities, IUnitOfWork unitOfWork) : ICommandHandler<DeleteCityCommand, DeleteCityCommandResult>
{
    private readonly ICityRepository _cities = cities;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<DeleteCityCommandResult> Handle(DeleteCityCommand command, CancellationToken cancellationToken)
    {
        var city = await _cities.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new AppException(ErrorCodes.CityNotFound);

        _cities.Remove(city);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new DeleteCityCommandResult();
    }
}

public record DeleteCityCommand(Guid Id) : ICommand<DeleteCityCommandResult>;

public record DeleteCityCommandResult;

public class DeleteCityCommandValidator : AbstractValidator<DeleteCityCommand>
{
    public DeleteCityCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithErrorCode(ErrorCodes.InvalidId.ToString());
    }
}
