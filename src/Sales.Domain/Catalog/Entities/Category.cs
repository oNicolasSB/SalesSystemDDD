using Sales.Domain.Catalog.Events;
using Sales.Domain.Common.Base;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Common.Validation;

namespace Sales.Domain.Catalog.Entities;

public sealed class Category : AggregateRoot
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }

    public Category(string name, string? description = null)
    {
        Guard.AgainstNullOrWhitespace(name, nameof(name));
        Guard.Against<DomainException>(name.Trim().Length < 3, $"{nameof(name)} cannot be less than 3 characters.");
        Name = name.Trim();
        Description = description;
        IsActive = true;
    }

    public void UpdateName(string newName)
    {
        Guard.AgainstNullOrWhitespace(newName, nameof(newName));
        Guard.Against<DomainException>(newName.Trim().Length < 3, $"{nameof(newName)} cannot be less than 3 characters.");
        Name = newName.Trim();
        UpdateDate();
    }

    public void UpdateDescription(string? newDescription)
    {
        Description = newDescription;
        UpdateDate();
    }

    public void Activate()
    {
        Guard.Against<DomainException>(IsActive, $"Category '{Name}' is already active.");

        IsActive = true;
        AddDomainEvent(new CategoryActivatedEvent(Id));
        UpdateDate();
    }

    public void Deactivate()
    {
        Guard.Against<DomainException>(!IsActive, $"Category '{Name}' is already inactive.");

        IsActive = false;
        AddDomainEvent(new CategoryDeactivatedEvent(Id));
        UpdateDate();
    }
}
