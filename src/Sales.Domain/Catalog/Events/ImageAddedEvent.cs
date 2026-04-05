using Sales.Domain.Common.Base;

namespace Sales.Domain.Catalog.Events;

public sealed record ImageAddedEvent(Guid ProductId, string ImageUrl, int Order) : DomainEventBase;
