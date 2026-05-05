namespace Sales.Application.Queries.Orders.Dtos;

public sealed class OrderSummaryDto
{
    public Guid Id { get; init; }
    public string OrderNumber { get; init; } = string.Empty;
    public Guid ClientId { get; init; }
    public decimal TotalValue { get; init; }
    public string OrderStatus { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public int TotalItems { get; init; }
    public int TotalPayments { get; init; }
}
