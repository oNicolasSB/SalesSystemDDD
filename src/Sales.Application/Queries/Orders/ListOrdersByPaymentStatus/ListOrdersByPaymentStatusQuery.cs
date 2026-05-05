using Sales.Domain.Orders.Enums;

namespace Sales.Application.Queries.Orders.ListOrdersByPaymentStatus;

public class ListOrdersByPaymentStatusQuery
{
    public PaymentStatus Status { get; }

    public ListOrdersByPaymentStatusQuery(PaymentStatus status)
    {
        Status = status;
    }
}
