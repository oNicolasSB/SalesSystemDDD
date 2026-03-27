using Sales.Domain.Common.Enums;
using Sales.Domain.ValueObjects;

namespace Sales.Domain.Events;

public sealed record class OrderCanceledEvent(
    Guid OrderId, 
    Guid ClientId, 
    OrderStatus PreviousStatus,
    CancelReason CancelReason,
    Guid? PaymentId) : DomainEventBase;