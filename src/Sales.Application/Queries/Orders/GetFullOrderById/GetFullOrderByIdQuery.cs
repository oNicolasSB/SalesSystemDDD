namespace Sales.Application.Queries.Orders.GetFullOrderById;

public sealed class GetFullOrderByIdQuery
{
    public Guid OrderId { get; }

    public GetFullOrderByIdQuery(Guid orderId)
    {
        if (orderId == Guid.Empty)
            throw new ArgumentException("Invalid OrderId.", nameof(orderId));

        OrderId = orderId;
    }
}
