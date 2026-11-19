using OrdersWorkerService.src.Infrastructure.Interfaces;

namespace OrdersWorkerService.src.Infrastructure.Hosting
{
    public class BackgroundWorker(IOrderConsumerService orderConsumer) : BackgroundService
    {
        private readonly IOrderConsumerService _orderConsumer = orderConsumer;
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _orderConsumer.StartAsync(stoppingToken);
            }
        }
    }
}