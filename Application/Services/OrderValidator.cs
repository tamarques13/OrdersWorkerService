
using OrdersWorkerService.Application.DTOs;

namespace OrdersWorkerService.Application.Validators;

public class OrderValidatorService
{
    public List<string> Validate(OrderMessageDto order)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(order.OrderId))
            errors.Add("OrderId is required.");
        else if (order.OrderId.Length > 100)
            errors.Add("OrderId cannot exceed 100 characters.");

        if (string.IsNullOrWhiteSpace(order.CustomerId))
            errors.Add("CustomerId is required.");
        else if (order.CustomerId.Length > 100)
            errors.Add("CustomerId cannot exceed 100 characters.");

        if (order.OrderDate == default)
            errors.Add("OrderDate is required.");

        if (order.Currency != "EUR")
            errors.Add("Currency must be EUR.");

        if (order.TotalAmount < 0)
            errors.Add("TotalAmount cannot be negative.");

        if (decimal.Round(order.TotalAmount, 2)
            != order.TotalAmount)
            errors.Add("TotalAmount can have at most 2 decimal places.");

        if (order.Items == null || order.Items.Count == 0)
        {
            errors.Add("The order must contain at least one item.");
            return errors;
        }

        decimal calculatedTotal = 0;

        foreach (var item in order.Items)
        {
            if (item == null)
            {
                errors.Add("The order contains a null item.");
                continue;
            }

            if (string.IsNullOrWhiteSpace(item.ProductId))
                errors.Add("ProductId is required.");
            else if (item.ProductId.Length > 100)
                errors.Add("ProductId cannot exceed 100 characters.");

            if (item.Quantity <= 0)
                errors.Add("Quantity must be greater than zero.");

            if (item.UnitPrice < 0)
                errors.Add("UnitPrice cannot be negative.");

            if (decimal.Round(item.UnitPrice, 2)
                != item.UnitPrice)
                errors.Add("UnitPrice can have at most 2 decimal places.");

            calculatedTotal += item.Quantity * item.UnitPrice;
        }

        if (calculatedTotal != order.TotalAmount)
            errors.Add("TotalAmount does not match the sum of the item amounts.");

        return errors;
    }
}
