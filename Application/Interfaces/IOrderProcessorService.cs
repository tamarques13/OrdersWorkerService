using OrdersWorkerService.Application.Enums;
using OrdersWorkerService.Application.DTOs;

namespace OrdersWorkerService.Application.Interfaces
{
    public interface IOrderProcessService
    {
        Task ProcessAsync(OrderMessageDto message, CancellationToken cancellationToken);
    }
}