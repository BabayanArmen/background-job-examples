## this is scheduled background service short example

Runs every 60 seconds
private readonly PeriodicTimer _timer = new(TimeSpan.FromMinutes(1));

protected override async Task ExecuteAsync(CancellationToken stoppingToken)
{
    while (await _timer.WaitForNextTickAsync(stoppingToken))
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();

        // Find tasks that are due and haven't been processed yet
        var dueTasks = await dbContext.ScheduledTasks
            .Where(t => !t.IsProcessed && t.ExecutionTime <= DateTime.UtcNow)
            .ToListAsync(stoppingToken);

        foreach (var task in dueTasks)
        {
            // Either process it right here, or push it into your existing immediate Channel queue!
            task.IsProcessed = true;
            // ... execute work ...
        }

        await dbContext.SaveChangesAsync(stoppingToken);
    }
}

## we can also have background task that strarts at specefic moment, for example on some api call, and run continuously