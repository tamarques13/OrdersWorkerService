using OrdersWorkerService.Application.DTOs;

namespace OrdersWorkerService.Application.Interfaces
{
    public interface IOrderValidationService
    {
        List<string> ValidateOrder(OrderMessageDto order);
    }
}