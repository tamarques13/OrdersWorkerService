namespace OrdersWorkerService.src.Infrastructure.Interfaces
{
    public interface IOrderConsumerService
    {
        Task StartAsync(CancellationToken stoppingToken);
    }
}