using MediatR;

namespace PersonDirectory.Shared.Infrastructure.Abstractios;

public interface IEventHandler<TEvent> : INotificationHandler<TEvent> where TEvent : IEvent;