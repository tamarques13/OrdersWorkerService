using OrdersWorkerService.src.Application.Enums;
using OrdersWorkerService.src.Infrastructure.Interfaces;
using OrdersWorkerService.src.Application.DTOs;
using OrdersWorkerService.src.Application.Interfaces;
using OrdersWorkerService.src.Domain.Entities;
using OrdersWorkerService.src.Domain.Enums;

namespace OrdersWorkerService.src.Application.Services
{
    public class OrderProcessService(
        IOrderRepository orderRepository,
        IOrderItemRepository orderItemRepository,
        ILogger<OrderProcessService> logger)
        : IOrderProcessService
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IOrderItemRepository _orderItemRepository = orderItemRepository;
        private readonly ILogger<OrderProcessService> _logger = logger;

        public async Task ProcessAsync(OrderMessageDto message, CancellationToken cancellationToken = default)
        {
            if (message.Currency != null && message.Items?.Count > 0)
            {

                var order = new Order(message.TotalAmount, Enum.Parse<CurrencyType>(message.Currency));
                var orderItem = new List<OrderItem>();

                foreach (var item in message.Items) orderItem.Add(new OrderItem(order.Id, item.Quantity, item.UnitPrice));

                try
                {
                    await _orderRepository.AddAsync(order, cancellationToken);

                    foreach (var item in orderItem) await _orderItemRepository.AddAsync(item, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to save order {OrderId}.", order.Id);
                    throw;
                }
            }
            else
            {
                _logger.LogError("Order Processing Failed.\n Must Provide a Currency and/or OrderItems");
            }
        }
    }
}