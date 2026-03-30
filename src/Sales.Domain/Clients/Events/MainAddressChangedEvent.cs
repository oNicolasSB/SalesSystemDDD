using Sales.Domain.Common.Base;

namespace Sales.Domain.Clients.Events;

public sealed record MainAddressChangedEvent(
    Guid ClientId,
    Guid NewAddressId) : DomainEventBase;
