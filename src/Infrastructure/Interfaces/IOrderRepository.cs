using OrdersWorkerService.src.Domain.Entities;

namespace OrdersWorkerService.src.Infrastructure.Interfaces
{
    public interface IOrderRepository
    {
       Task AddAsync(Order order, CancellationToken cancellationToken);
    }
}