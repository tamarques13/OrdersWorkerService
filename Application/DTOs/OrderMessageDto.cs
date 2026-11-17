
namespace OrdersWorkerService.Application.DTOs;

public class OrderMessageDto
{
    public string? OrderId { get; set; }

    public DateTimeOffset OrderDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string? Currency { get; set; }

    public List<OrderItemDto>? Items { get; set; }
}
