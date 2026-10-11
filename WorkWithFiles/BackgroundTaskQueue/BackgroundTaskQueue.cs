using System.Threading.Channels;
using WorkWithFiles.Models;

namespace WorkWithFiles.BackgroundTaskQueue
{
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly Channel<ScheduledTask> _taskQueue;

        public BackgroundTaskQueue()
        {
            var options = new BoundedChannelOptions(100)
            {
                FullMode = BoundedChannelFullMode.Wait
            };

            _taskQueue = Channel.CreateBounded<ScheduledTask>(options);
        }

        public async ValueTask<ScheduledTask> DequeueAsync(CancellationToken cancellationToken)
        {
            return await _taskQueue.Reader.ReadAsync(cancellationToken);
        }

        public async ValueTask EnqueueAsync(ScheduledTask scheduledTask)
        {
            if (scheduledTask is not null)
            {
                await _taskQueue.Writer.WriteAsync(scheduledTask);
            }
        }
    }
}
