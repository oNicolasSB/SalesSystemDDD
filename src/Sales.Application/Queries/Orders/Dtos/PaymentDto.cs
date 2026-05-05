namespace Sales.Application.Queries.Orders.Dtos;

public sealed class PaymentDto
{
    public Guid PaymentId { get; init; }
    public string PaymentStatus { get; init; } = string.Empty;
    public string PaymentMethod { get; init; } = string.Empty;
    public decimal Value { get; init; }
    public string? TransactionCode { get; init; }
    public DateTime? PaidAt { get; init; }
}
