using OrdersWorkerService.Domain.Entities;

namespace OrdersWorkerService.Infrastructure.Interfaces
{
    public interface IOrderItemRepository
    {
        Task AddAsync(OrderItem orderItem, CancellationToken cancellationToken);
    }
}