using Sales.Domain.Common.Base;

namespace Sales.Domain.Catalog.Events;

public sealed record ProductDeactivatedEvent(Guid ProductId) : DomainEventBase;