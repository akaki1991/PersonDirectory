using MediatR;

namespace PersonDirectory.Shared.Infrastructure.Abstractios;

public interface IQuery<out TResponse> : IRequest<TResponse> where TResponse : notnull;
