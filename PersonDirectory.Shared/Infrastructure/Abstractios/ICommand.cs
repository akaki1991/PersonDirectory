using MediatR;

namespace PersonDirectory.Shared.Infrastructure.Abstractios;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}
