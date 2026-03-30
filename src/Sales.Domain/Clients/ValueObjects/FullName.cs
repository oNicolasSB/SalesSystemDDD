using Sales.Domain.Common.Base;
using Sales.Domain.Common.Validation;

namespace Sales.Domain.Clients.ValueObjects;

public sealed class FullName : ValueObject
{
    public string Name { get; }
    public string LastName { get; }
    public string FormatedFullName { get; }

    public FullName(string fullName)
    {
        Guard.AgainstNullOrWhitespace(fullName, nameof(fullName));
        var parts = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        Guard.Against<ArgumentException>(parts.Length < 2, "Full name must contain at least a name and a last name.");

        LastName = parts.Last();
        Name = string.Join(' ', parts.Take(parts.Length - 1));
        FormatedFullName = string.Join(' ', parts);
    }

    public string AbbreviatedName() => $"{Name.Split(' ').FirstOrDefault()} {LastName}";

    public override string ToString() => FormatedFullName;


    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FormatedFullName.ToLowerInvariant();
    }
}
