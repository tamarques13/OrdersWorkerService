using OrdersWorkerService.Application.DTOs;

namespace OrdersWorkerService.Application.Interfaces
{
    public interface IOrderValidatorService
    {
        List<string> Validate(OrderMessageDto order);
    }
}