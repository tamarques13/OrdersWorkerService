namespace OrdersWorkerService.Infrastructure.Interfaces
{
    public interface IOrderConsumerService
    {
        Task StartAsync(CancellationToken stoppingToken);
    }
}