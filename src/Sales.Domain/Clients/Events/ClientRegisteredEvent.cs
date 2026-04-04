using Sales.Domain.Common.Base;

namespace Sales.Domain.Clients.Events;

public sealed record ClientRegisteredEvent(
    Guid ClientId,
    string Name,
    string Cpf,
    string Email) : DomainEventBase;
