namespace Sales.Domain.Common.Interfaces;

public interface IDomainEvent
{
    public DateTime DateOccurred { get; }
}
