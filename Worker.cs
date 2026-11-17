using OrdersWorkerService.Services;

namespace OrdersWorkerService
{
    public class Worker(OrderConsumerService orderConsumer) : BackgroundService
    {
        private readonly OrderConsumerService _orderConsumer = orderConsumer;
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _orderConsumer.StartAsync(stoppingToken);
            }
        }
    }
}