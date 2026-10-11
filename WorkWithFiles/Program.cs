using System.ComponentModel.Design;
using WorkWithFiles.BackgroundTaskQueue;
using WorkWithFiles.Services;
using WorkWithFiles.Services.HttpService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.Configure<HostOptions>(x =>
//{
//    x.ServicesStartConcurrently = true;
//    x.ServicesStopConcurrently = false;
//});

//builder.Services.AddHostedService<FetchDataBackgroundService>();

builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();

builder.Services.AddHostedService<BackgroundTaskService>();

builder.Services.AddScoped(typeof(IHttpService<>), typeof(HttpService<>));

builder.Services.AddHttpClient();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
