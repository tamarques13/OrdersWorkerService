namespace OrdersWorkerService.Application.Interfaces
{
    public interface IOrderConsumerService
    {
        Task StartAsync(CancellationToken stoppingToken);
    }
}