using OrdersWorkerService.src.Application.DTOs;

namespace OrdersWorkerService.src.Application.Interfaces
{
    public interface IOrderValidationService
    {
        List<string> ValidateOrder(OrderMessageDto order);
    }
}