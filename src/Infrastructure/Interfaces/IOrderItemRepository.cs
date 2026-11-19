using OrdersWorkerService.src.Domain.Entities;

namespace OrdersWorkerService.src.Infrastructure.Interfaces
{
    public interface IOrderItemRepository
    {
        Task AddAsync(OrderItem orderItem, CancellationToken cancellationToken);
    }
}