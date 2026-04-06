using Sales.Domain.Common.Base;

namespace Sales.Domain.Catalog.Events;

public sealed record ProductActivatedEvent(Guid ProductId) : DomainEventBase;
