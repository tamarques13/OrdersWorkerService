
using OrdersWorkerService.Application.Interfaces;
using OrdersWorkerService.Application.DTOs;

namespace OrdersWorkerService.Application.Services
{
    public class OrderValidatorService : IOrderValidatorService
    {
        private const int MaxIdLength = 100;
        private const string SupportedCurrency = "EUR";
        private const int MaxDecimalPlaces = 2;

        public List<string> Validate(OrderMessageDto order)
        {
            ArgumentNullException.ThrowIfNull(order);

            var errors = new List<string>();

            ValidateOrder(order, errors);

            if (order.Items == null || order.Items.Count == 0)
            {
                errors.Add("The order must contain at least one item.");
                return errors;
            }

            ValidateItems(order, errors);

            return errors;
        }

        private static void ValidateOrder(OrderMessageDto order, List<string> errors)
        {
            ValidateRequiredId(order.OrderId, nameof(order.OrderId), errors);
            ValidateRequiredId(order.CustomerId, nameof(order.CustomerId), errors);

            if (order.OrderDate == default)
                errors.Add("OrderDate is required.");

            // HERE USE THE ENUM IN DOMAIN
            if (order.Currency != SupportedCurrency)
                errors.Add($"Currency must be {SupportedCurrency}.");

            ValidateAmount(order.TotalAmount, nameof(order.TotalAmount), errors);
        }

        private static void ValidateItems(OrderMessageDto order, List<string> errors)
        {
            decimal calculatedTotal = 0;

            foreach (var item in order.Items!)
            {
                if (item == null)
                {
                    errors.Add("The order contains a null item.");
                    continue;
                }

                ValidateRequiredId(item.ProductId, nameof(item.ProductId), errors);

                if (item.Quantity <= 0)
                    errors.Add("Quantity must be greater than zero.");

                ValidateAmount(item.UnitPrice, nameof(item.UnitPrice), errors);

                calculatedTotal += item.Quantity * item.UnitPrice;
            }

            if (calculatedTotal != order.TotalAmount)
                errors.Add("TotalAmount does not match the sum of the item amounts.");
        }

        private static void ValidateRequiredId(string? value, string fieldName, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(value))
                errors.Add($"{fieldName} is required.");

            else if (value.Length > MaxIdLength)
                errors.Add($"{fieldName} cannot exceed {MaxIdLength} characters.");
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
