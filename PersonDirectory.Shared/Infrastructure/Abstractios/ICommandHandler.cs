using MediatR;

namespace PersonDirectory.Shared.Infrastructure.Abstractios;

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
           where TCommand : ICommand<TResponse>
           where TResponse : notnull;
