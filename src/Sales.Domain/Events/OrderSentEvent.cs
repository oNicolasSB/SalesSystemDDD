using Sales.Domain.ValueObjects;

namespace Sales.Domain.Events;

public sealed record class OrderSentEvent(Guid OrderId, Guid ClientId, DeliveryAddress DeliveryAddress) : DomainEventBase;