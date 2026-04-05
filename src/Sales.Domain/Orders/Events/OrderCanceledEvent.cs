using Sales.Domain.Clients.Enums;
using Sales.Domain.Common.Base;
using Sales.Domain.Orders.ValueObjects;

namespace Sales.Domain.Orders.Events;

public sealed record class OrderCanceledEvent(
    Guid OrderId, 
    Guid ClientId, 
    OrderStatus PreviousStatus,
    CancelReason CancelReason,
    Guid? PaymentId) : DomainEventBase;