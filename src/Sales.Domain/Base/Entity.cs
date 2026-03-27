using Sales.Domain.Events;

namespace Sales.Domain.Base;

public abstract class Entity
{
    public Guid Id { get; protected set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    protected Entity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }
    protected Entity(Guid id)
    {
        Id = id;
    }
    protected void UpdateDate()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        return Id == other.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static bool operator ==(Entity left, Entity right)
    {
        if (left is null)
            return right is null;

        return left.Equals(right);
    }

    public static bool operator !=(Entity left, Entity right)
    {
        return !(left == right);
    }

    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
    protected void RemoveDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    protected void ClearDomainEvents() => _domainEvents.Clear();
}
