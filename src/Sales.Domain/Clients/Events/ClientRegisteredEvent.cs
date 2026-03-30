using Sales.Domain.Common.Base;

namespace Sales.Domain.Clients.Events;

public sealed record ClientRegisteredEvent(
    Guid ClientId,
    string Nome,
    string Cpf,
    string Email) : DomainEventBase;
