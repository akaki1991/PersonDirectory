using MediatR;
using PersonDirectory.Shared.Extensions;
using PersonDirectory.Shared.Infrastructure.Abstractios;

namespace PersonDirectory.Shared.Infrastructure;

public class DomainEventsDispatcher(IMediator mediator, IDbContext db) : IDomainEventsDispatcher
{
    private readonly IMediator _mediator = mediator;
    private readonly IDbContext _db = db;

    public async Task DispatchEventsAsync()
    {
        var modifiedAggregates = _db.ChangeTracker.Entries<IHasDomainEvents>().ToList();

        foreach (var entry in modifiedAggregates)
        {
            var aggregateEvents = entry.Entity.UncommittedChanges();

            await _mediator.PublishMultipleAsync(aggregateEvents);

            entry.Entity.MarkChangesAsCommitted();
        }
    }
}
