using MediatR;

namespace PersonDirectory.Shared.Extensions;

public static class MediatorExtensions
{
    public static async Task PublishMultipleAsync(this IMediator mediator, IEnumerable<object> events, CancellationToken cancellationToken = default)
    {
        var publishTasks = events.Select(e => mediator.Publish(e, cancellationToken));

        foreach (var publishTask in publishTasks)
        {
            await publishTask;
        }
    }
}
