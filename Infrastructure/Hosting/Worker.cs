using OrdersWorkerService.Infrastructure.Interfaces;

namespace OrdersWorkerService.Infrastructure.Hosting
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