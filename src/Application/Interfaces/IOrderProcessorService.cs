using OrdersWorkerService.src.Application.Enums;
using OrdersWorkerService.src.Application.DTOs;

namespace OrdersWorkerService.src.Application.Interfaces
{
    public interface IOrderProcessService
    {
        Task ProcessAsync(OrderMessageDto message, CancellationToken cancellationToken);
    }
}