using Injectio.Attributes;

namespace ServerSentMeAnAngel;

// similar to AddHostedService
[RegisterSingleton<IHostedService>(Duplicate = DuplicateStrategy.Append)]
public class FoodServiceWorker(FoodService foodService)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            foodService.Set();
            await Task.Delay(1000, stoppingToken);
        }
    }
}