using Sales.Domain.Common.Base;

namespace Sales.Domain.Clients.Events;

public sealed record ClientBlockedEvent(
    Guid ClientId,
    string Cpf) : DomainEventBase;