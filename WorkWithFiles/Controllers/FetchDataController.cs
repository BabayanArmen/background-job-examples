using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;
using System.Text.Json;
using WorkWithFiles.BackgroundTaskQueue;
using WorkWithFiles.Models;
using WorkWithFiles.Services.HttpService;

namespace WorkWithFiles.Controllers
{
    [Route("api/fetch")]
    [ApiController]
    public class FetchDataController
        (
            IHttpService<PhotoDto> _httpService, 
            ILogger<FetchDataController> _logger, 
            IConfiguration _configuration,
            IBackgroundTaskQueue _taskQueue
        ) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken, [FromQuery] PhotoFilter? filter)
        {
            var res = await _httpService.GetAll("https://jsonplaceholder.typicode.com/photos", cancellationToken, filter);

            if (res is not null)
            {
                //_logger.LogInformation(JsonSerializer.Serialize(res));

                string path = _configuration["FileSettings:FilePath"];

                var filePath = Path.Combine(path!, "data.txt");

                await using var writer = new StreamWriter(filePath, append: true);

                foreach (var line in res)
                {
                    await writer.WriteLineAsync(JsonSerializer.Serialize(line));
                }

                return Ok(res);
            }

            return NotFound();
        }

        [HttpPost("task")]
        public async Task<IActionResult> SceduleTask([FromBody] ScheduledTask scheduledTask)
        {
            await _taskQueue.EnqueueAsync(scheduledTask);
            return Ok("Task Schedulled");
        }
    }
}
