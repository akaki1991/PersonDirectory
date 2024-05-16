using Microsoft.EntityFrameworkCore;
using PersonDirectory.Domain.PersonManagement.Events;
using PersonDirectory.Domain.PersonManagement.ReadModels;
using PersonDirectory.Infrastructure.DataAccess;
using PersonDirectory.Shared.Infrastructure.Abstractios;

namespace PersonDirectory.Application.DomainEventHandlers;

public class PersonEventHandler(PersonDirectoryDbContext db) :
    IEventHandler<PersonCreatedEvent>,
    IEventHandler<PersonChangedEvent>,
    IEventHandler<PersonDeletedEvent>
{
    private readonly PersonDirectoryDbContext _db = db;

    public async Task Handle(PersonCreatedEvent @event, CancellationToken cancellationToken)
    {
        var personReadModel = PersonReadModel.From(@event);
        await _db.AddAsync(personReadModel, cancellationToken);
    }

    public async Task Handle(PersonChangedEvent @event, CancellationToken cancellationToken)
    {
        var personReadModel = await _db.PersonReadModels
            .FirstOrDefaultAsync(x => x.PersonId == @event.AggregateRootId, cancellationToken);

        if (personReadModel is null)
            return;

        personReadModel.FirstName = @event.FirstName;
        personReadModel.LastName = @event.LastName;
        personReadModel.Gender = @event.Gender;
        personReadModel.PersonalNumber = @event.PersonalNumber;
        personReadModel.DateOfBirth = @event.DateOfBirth;
        personReadModel.City = @event.Address?.City;
        personReadModel.PhoneNumbers = @event.PhoneNumbers;
        personReadModel.Photo = @event.Photo;
    }

    public async Task Handle(PersonDeletedEvent @event, CancellationToken cancellationToken)
    {
        var personReadModel = await _db.PersonReadModels
            .FirstOrDefaultAsync(x => x.PersonId == @event.AggregateRootId, cancellationToken);

        if (personReadModel is not null)
            _db.PersonReadModels.Remove(personReadModel);
    }
}
