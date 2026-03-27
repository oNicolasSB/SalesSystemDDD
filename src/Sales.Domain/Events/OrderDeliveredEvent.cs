namespace Sales.Domain.Events;

public sealed record class OrderDeliveredEvent(Guid OrderId, Guid ClientId) : DomainEventBase;
