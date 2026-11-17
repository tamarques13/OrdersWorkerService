using OrdersWorkerService.Domain.Entities;

namespace OrdersWorkerService.Infrastructure.Interfaces
{
    public interface IOrderRepository
    {
       Task AddAsync(Order order);
    }
}