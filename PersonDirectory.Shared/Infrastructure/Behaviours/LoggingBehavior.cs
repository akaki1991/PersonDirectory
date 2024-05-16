using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace PersonDirectory.Shared.Infrastructure.Behaviours;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Pre-processing
        _logger.LogInformation("Handling request = {request} - Data  = {requestData}", typeof(TRequest).Name, request);

        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var response = await next();

        stopwatch.Stop();
        var takenTime = stopwatch.Elapsed.Seconds;

        if (takenTime > 2)
            logger.LogWarning("Request {Request} took {TakenTime} seconds.",
                typeof(TRequest).Name, takenTime);

        // Post-processing
        _logger.LogInformation("Handled {request} - with {response}", typeof(TRequest).Name, typeof(TResponse).Name);

        return response;
    }
}
