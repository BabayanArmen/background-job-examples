using WorkWithFiles.Models;

namespace WorkWithFiles.BackgroundTaskQueue
{
    public interface IBackgroundTaskQueue
    {
        ValueTask EnqueueAsync(ScheduledTask scheduledTask);
        ValueTask<ScheduledTask> DequeueAsync(CancellationToken cancellationToken);
    }
}
