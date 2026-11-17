using OrdersWorkerService.Domain.Entities;
using OrdersWorkerService.Infrastructure.Interfaces;
using OrdersWorkerService.Infrastructure.Data;

namespace OrdersWorkerService.Infrastructure.Repositories
{
    public class OrderItemRepository(AppDbContext dbContext) : IOrderItemRepository
    {
        private AppDbContext _dbContext = dbContext;

        public async Task AddAsync(OrderItem orderItem)
        {
            await _dbContext.OrderItems.AddAsync(orderItem);

            await _dbContext.SaveChangesAsync();
        }
    }
}