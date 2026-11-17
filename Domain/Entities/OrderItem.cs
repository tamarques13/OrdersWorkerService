namespace OrdersWorkerService.Domain.Entities
{
    public class OrderItem
    {
        public Guid Id { get; private set; }
        public Guid OrderId { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public Order Order { get; set; } = null!;

        private OrderItem() { }
        public OrderItem(Guid orderId, int quantity, decimal unitPrice)
        {
            Id = Guid.NewGuid();
            OrderId = orderId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }
    }
}