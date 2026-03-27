namespace Sales.Domain.Base;

public class AggregateRoot : Entity
{
    protected AggregateRoot() : base() { }
    protected AggregateRoot(Guid id) : base(id) { }
}
