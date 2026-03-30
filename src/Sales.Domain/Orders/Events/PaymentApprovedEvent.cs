using Sales.Domain.Common.Base;

namespace Sales.Domain.Orders.Events;

public record PaymentConfirmedEvent(
    Guid PaymentId,
    Guid OrderId,
    decimal Value,
    DateTime PaymentDate,
    string? TransactionCode) : DomainEventBase;
