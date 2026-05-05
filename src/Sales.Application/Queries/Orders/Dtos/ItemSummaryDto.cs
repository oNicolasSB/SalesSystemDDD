namespace Sales.Application.Queries.Orders.Dtos;

public sealed class ItemSummaryDto
{
    public Guid ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public decimal UnitPrice { get; init; }
    public int Quantity { get; init; }
    public decimal TotalValue { get; init; }
}
