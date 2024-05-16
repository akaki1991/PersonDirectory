using FluentValidation;
using PersonDirectory.Application.Shared;
using PersonDirectory.Domain.CityManagement;
using PersonDirectory.Domain.CityManagement.Repositories;
using PersonDirectory.Shared;
using PersonDirectory.Shared.Infrastructure.Abstractios;

namespace PersonDirectory.Application.CityManagement.Commands;

public class AddCityCommandHandler(ICityRepository cities, IUnitOfWork unitOfWork) : ICommandHandler<AddCityCommand, AddCityCommandResult>
{
    private readonly ICityRepository _cities = cities;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<AddCityCommandResult> Handle(AddCityCommand command, CancellationToken cancellationToken)
    {
        var city = City.With(name: command.Name);

        await _cities.AddAsync(city, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new AddCityCommandResult(city.Id);
    }
}

public record AddCityCommand(string Name) : ICommand<AddCityCommandResult>;

public record AddCityCommandResult(Guid Id);

public class AddCityCommandValidator : AbstractValidator<AddCityCommand>
{
    public AddCityCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithErrorCode(ErrorCodes.InvalidCityName.ToString());
    }
}
