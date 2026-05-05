namespace Sales.Application.Queries.Orders.Dtos;

public sealed class PaymentByStatusDto
{
    public Guid PaymentId { get; init; }
    public string OrderNumber { get; init; } = string.Empty;
    public Guid ClientId { get; init; }
    public decimal TotalValue { get; init; }
    public string PaymentStatus { get; init; } = string.Empty;
    public string PaymentMethod { get; init; } = string.Empty;
    public string? TransactionCode { get; init; }
    public DateTime? PaidAt { get; init; }
}
