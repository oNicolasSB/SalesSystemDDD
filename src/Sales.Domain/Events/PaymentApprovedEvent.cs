namespace Sales.Domain.Events;

public record PaymentConfirmedEvent(
    Guid PaymentId,
    Guid OrderId,
    decimal Value,
    DateTime PaymentDate,
    string? TransactionCode) : DomainEventBase;
