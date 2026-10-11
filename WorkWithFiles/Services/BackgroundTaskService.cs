using System.Text.Json;
using WorkWithFiles.BackgroundTaskQueue;
using WorkWithFiles.Models;
using WorkWithFiles.Services.HttpService;

namespace WorkWithFiles.Services
{
    public class BackgroundTaskService : BackgroundService
    {
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<BackgroundTaskService> _logger;

        public BackgroundTaskService
            (
                IBackgroundTaskQueue taskQueue, 
                IServiceScopeFactory serviceScopeFactory, 
                ILogger<BackgroundTaskService> logger
            )
        {
            _taskQueue = taskQueue;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {


            while (!stoppingToken.IsCancellationRequested)
            {

                var scheduledTask = await _taskQueue.DequeueAsync(stoppingToken);

                try
                {
                    using var scope = _serviceScopeFactory.CreateScope();

                    var _httpService = scope.ServiceProvider.GetRequiredService<IHttpService<PhotoDto>>();

                    var res = await _httpService.GetAll($"https://jsonplaceholder.typicode.com/photos?_page={scheduledTask.PhotoID.ToString()}&_limit=1", stoppingToken);

                    if (res is not null)
                    {
                        _logger.LogInformation($"{scheduledTask.Message} with id {scheduledTask.PhotoID} is completed at {DateTime.Now}");
                        _logger.LogInformation(JsonSerializer.Serialize(res));
                    }

                }
                catch ( Exception ex )
                {
                    _logger?.LogInformation(ex.Message);
                }
            }
        }
    }
}
