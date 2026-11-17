
using OrdersWorkerService.DTOs;

namespace OrdersWorkerService.Validators;

public class OrderValidator
{
    public List<string> Validate(OrderMessageDto order)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(order.OrderId))
            errors.Add("OrderId é obrigatório.");
        else if (order.OrderId.Length > 100)
            errors.Add("OrderId não pode exceder 100 caracteres.");

        if (string.IsNullOrWhiteSpace(order.CustomerId))
            errors.Add("CustomerId é obrigatório.");
        else if (order.CustomerId.Length > 100)
            errors.Add("CustomerId não pode exceder 100 caracteres.");

        if (order.OrderDate == default)
            errors.Add("OrderDate é obrigatória.");

        if (order.Currency != "EUR")
            errors.Add("A moeda deve ser EUR.");

        if (order.TotalAmount < 0)
            errors.Add("TotalAmount não pode ser negativo.");

        if (decimal.Round(order.TotalAmount, 2)
            != order.TotalAmount)
            errors.Add("TotalAmount só pode ter 2 casas decimais.");

        if (order.Items == null || order.Items.Count == 0)
        {
            errors.Add("A encomenda deve ter pelo menos um produto.");
            return errors;
        }

        decimal calculatedTotal = 0;

        foreach (var item in order.Items)
        {
            if (item == null)
            {
                errors.Add("A encomenda contém um produto nulo.");
                continue;
            }

            if (string.IsNullOrWhiteSpace(item.ProductId))
                errors.Add("ProductId é obrigatório.");
            else if (item.ProductId.Length > 100)
                errors.Add("ProductId não pode exceder 100 caracteres.");

            if (item.Quantity <= 0)
                errors.Add("Quantity deve ser maior que zero.");

            if (item.UnitPrice < 0)
                errors.Add("UnitPrice não pode ser negativo.");

            if (decimal.Round(item.UnitPrice, 2)
                != item.UnitPrice)
                errors.Add("UnitPrice só pode ter 2 casas decimais.");

            calculatedTotal += item.Quantity * item.UnitPrice;
        }

        if (calculatedTotal != order.TotalAmount)
            errors.Add("O total da encomenda não corresponde à soma dos produtos.");

        return errors;
    }
}
