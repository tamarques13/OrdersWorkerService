using OrdersWorkerService.Domain.Entities;
using OrdersWorkerService.Infrastructure.Interfaces;
using OrdersWorkerService.Infrastructure.Data;

namespace OrdersWorkerService.Infrastructure.Repositories
{
    public class OrderRepository(AppDbContext dbContext) : IOrderRepository
    {
        private AppDbContext _dbContext = dbContext;

        public async Task AddAsync(Order order, CancellationToken cancellationToken)
        {
            try
            {
                await _dbContext.Orders.AddAsync(order, cancellationToken);
                
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _dbContext.ChangeTracker.Clear();
                throw new Exception("Failed to Save Order in Database.\n Error: \n -", ex);
            }
        }
    }
}