using System.ComponentModel.Design;
using System.Text.Json;
using WorkWithFiles.Models;
using WorkWithFiles.Services.HttpService;
//using static System.Net.WebRequestMethods;

namespace WorkWithFiles.Services
{
    public class FetchDataBackgroundService : BackgroundService
    {
        private readonly TimeSpan _period = TimeSpan.FromSeconds(3);

        private ILogger<FetchDataBackgroundService> _logger;

        private IServiceScopeFactory _serviceScopeFactory;
        public FetchDataBackgroundService(ILogger<FetchDataBackgroundService> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        } 
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            //Console.WriteLine("start wait");
            //await Task.Delay(20000);
            //Console.WriteLine("finish wait");

            using var timer = new PeriodicTimer(_period);

            using var scope = _serviceScopeFactory.CreateScope();

            try
            {
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    _logger.LogInformation($"background fetch is worked at {DateTime.Now}");

                    var _httpService = scope.ServiceProvider.GetRequiredService<IHttpService<PhotoDto>>();

                    var res = await _httpService.GetAll("https://jsonplaceholder.typicode.com/photos?_page=1&_limit=1", stoppingToken);

                    if (res != null)
                    {
                        _logger.LogInformation(JsonSerializer.Serialize(res));
                    }

                }
            }
            catch (OperationCanceledException ex)
            {
                _logger?.LogInformation(ex.Message);
            }

        }
    }
}
