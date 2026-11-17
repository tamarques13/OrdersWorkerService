using OrdersWorkerService.Domain.Entities;
using OrdersWorkerService.Infrastructure.Interfaces;
using OrdersWorkerService.Infrastructure.Data;

namespace OrdersWorkerService.Infrastructure.Repositories
{
    public class OrderItemRepository(AppDbContext dbContext) : IOrderItemRepository
    {
        private AppDbContext _dbContext = dbContext;

        public async Task AddAsync(OrderItem orderItem, CancellationToken cancellationToken)
        {
            try
            {
                await _dbContext.OrderItems.AddAsync(orderItem, cancellationToken);

                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _dbContext.ChangeTracker.Clear();
                throw new Exception("Failed to Save OrderItem in Database.\n Error: \n -", ex);
            }
        }
    }
}       