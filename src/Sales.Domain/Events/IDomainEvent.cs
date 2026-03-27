namespace Sales.Domain.Events;

public interface IDomainEvent
{
    public DateTime DateOccurred { get; }
}
