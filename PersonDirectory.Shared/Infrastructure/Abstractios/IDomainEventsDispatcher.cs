namespace PersonDirectory.Shared.Infrastructure.Abstractios;

public interface IDomainEventsDispatcher
{
    Task DispatchEventsAsync();
}
