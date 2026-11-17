using OrdersWorkerService.Domain.Enums;

namespace OrdersWorkerService.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; private set; }
        public DateTime OrderDate { get; private set; }
        public decimal TotalAmount { get; private set; }
        public CurrencyType Currency { get; private set; }
        public List<OrderItem> Items { get; private set; } = new();

        private Order() { }

        public Order(decimal totalAmount, CurrencyType currencytype)
        {
            if (totalAmount < 0) throw new ArgumentOutOfRangeException(nameof(totalAmount));

            if (!Enum.IsDefined(typeof(CurrencyType), currencytype)) throw new ArgumentException($"Invalid Currency type: {currencytype}");

            Id = Guid.NewGuid();
            OrderDate = DateTime.UtcNow;
            TotalAmount = totalAmount;
            Currency = currencytype;
        }
    }
}