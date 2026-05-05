namespace Sales.Application.Queries.Orders.ListOrdersByClientSummary;

public sealed class ListOrdersByClientSummaryQuery
{
    public ListOrdersByClientSummaryQuery(Guid clientId)
    {
        if (clientId == Guid.Empty)
            throw new ArgumentException("Invalid ClientId.", nameof(clientId));

        ClientId = clientId;
    }

    public Guid ClientId { get; }


}
