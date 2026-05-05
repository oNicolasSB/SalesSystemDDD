namespace Sales.Application.Queries.Orders.Dtos;

public sealed class OrderFullDto
{
    public Guid Id { get; init; }
    public string OrderNumber { get; init; } = string.Empty;
    public Guid ClientId { get; init; }
    public decimal TotalValue { get; init; }
    public string OrderStatus { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public DeliveryAddressDto Address { get; init; } = null!;
    public IReadOnlyList<ItemSummaryDto> Items { get; init; } = [];
    public IReadOnlyList<PaymentDto> Payments { get; init; } = [];
}
