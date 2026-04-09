using Sales.Domain.Orders.Enums;

namespace Sales.Application.Commands.OrdersCommands.StartPayment;

public sealed class StartPaymentCommand
{
    public Guid OrderId { get; }
    public PaymentMethod PaymentMethod { get; }

    public StartPaymentCommand(Guid orderId, PaymentMethod paymentMethod)
    {
        OrderId = orderId;
        PaymentMethod = paymentMethod;
    }
}
