using OrdersWorkerService.src.Domain.Entities;
using OrdersWorkerService.src.Infrastructure.Data;
using OrdersWorkerService.src.Infrastructure.Interfaces;

namespace OrdersWorkerService.src.Infrastructure.Repositories
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