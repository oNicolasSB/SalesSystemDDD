using Sales.Domain.Common.Base;

namespace Sales.Domain.Catalog.Events;

public sealed record CategoryDeactivatedEvent(Guid CategoryId) : DomainEventBase;
