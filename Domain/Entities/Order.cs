using OrdersWorkerService.Domain.Enums;

namespace OrdersWorkerService.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; private set; }
        public DateTime OrderDate { get; private set; }
        public decimal TotalAmount { get; private set; }
        public CurrencyType Currency { get; private set; }
        public List<OrderItem> Items { get; private set; }

        private Order() { }
        public Order(decimal totalAmount, CurrencyType currencytype)
        {
            Id = Guid.NewGuid();
            OrderDate = DateTime.Now;
            TotalAmount = totalAmount;
            Currency = currencytype;
        }
    }
}