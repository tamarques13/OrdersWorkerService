using OrdersWorkerService.Domain.Entities;
using OrdersWorkerService.Infrastructure.Interfaces;
using OrdersWorkerService.Infrastructure.Data;

namespace OrdersWorkerService.Infrastructure.Repositories
{
    public class OrderRepository(AppDbContext dbContext) : IOrderRepository
    {
        private AppDbContext _dbContext = dbContext;

        public async Task AddAsync(Order order)
        {
            await _dbContext.Orders.AddAsync(order);

            await _dbContext.SaveChangesAsync();
        }
    }
}