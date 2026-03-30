using Sales.Domain.Common.Base;
using Sales.Domain.Orders.ValueObjects;

namespace Sales.Domain.Orders.Events;

public sealed record class OrderSentEvent(Guid OrderId, Guid ClientId, DeliveryAddress DeliveryAddress) : DomainEventBase;