using Sales.Domain.Common.Base;

namespace Sales.Domain.Orders.Events;

public sealed record class OrderDeliveredEvent(Guid OrderId, Guid ClientId) : DomainEventBase;
