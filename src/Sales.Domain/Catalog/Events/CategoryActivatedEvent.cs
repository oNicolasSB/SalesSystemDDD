using Sales.Domain.Common.Base;

namespace Sales.Domain.Catalog.Events;

public sealed record CategoryActivatedEvent(Guid CategoryId) : DomainEventBase;
