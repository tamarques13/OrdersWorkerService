
using OrdersWorkerService.Application.Interfaces;
using OrdersWorkerService.Application.DTOs;
using OrdersWorkerService.Domain.Enums;

namespace OrdersWorkerService.Application.Services
{
    public class OrderValidationService : IOrderValidationService
    {
        private const int MaxDecimalPlaces = 2;

        public List<string> ValidateOrder(OrderMessageDto order)
        {
            ArgumentNullException.ThrowIfNull(order);

            var errors = new List<string>();

            ValidateOrderFields(order, errors);

            if (order.Items == null || order.Items.Count == 0)
            {
                errors.Add("The order must contain at least one item.");
                return errors;
            }

            ValidateOrderItems(order, errors);

            return errors;
        }

        private static void ValidateOrderFields(OrderMessageDto order, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(order.OrderId))
                errors.Add("OrderId is required.");

            if (!Enum.TryParse<CurrencyType>(order.Currency, out var currency)
                || !Enum.IsDefined(currency))
            {
                errors.Add($"Currency must be a supported value: {string.Join(", ", Enum.GetNames<CurrencyType>())}.");
            }

            ValidateAmount(order.TotalAmount, nameof(order.TotalAmount), errors);
        }

        private static void ValidateOrderItems(OrderMessageDto order, List<string> errors)
        {
            decimal calculatedTotal = 0;

            foreach (var item in order.Items!)
            {
                if (item == null)
                {
                    errors.Add("The order contains a null item.");
                    continue;
                }

                if (item.Quantity <= 0)
                    errors.Add("Quantity must be greater than zero.");

                ValidateAmount(item.UnitPrice, nameof(item.UnitPrice), errors);

                calculatedTotal += item.Quantity * item.UnitPrice;
            }

            if (calculatedTotal != order.TotalAmount)
                errors.Add("TotalAmount does not match the sum of the item amounts.");
        }

        private static void ValidateAmount(decimal amount, string fieldName, List<string> errors)
        {
            if (amount < 0)
                errors.Add($"{fieldName} cannot be negative.");

            if (decimal.Round(amount, MaxDecimalPlaces) != amount)
                errors.Add($"{fieldName} can have at most {MaxDecimalPlaces} decimal places.");
        }
    }
}
