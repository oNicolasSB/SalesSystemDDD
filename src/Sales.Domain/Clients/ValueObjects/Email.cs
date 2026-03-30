using System.Text.RegularExpressions;
using Sales.Domain.Common.Base;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Common.Validation;

namespace Sales.Domain.Clients.ValueObjects;

public sealed class Email : ValueObject
{
    public string Address { get; }

    private static readonly Regex _regex = new(
        @"^[\w\.-]+@[\w\.-]+\.\w{2,}$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase
    );

    public Email(string emailAddress)
    {
        Guard.AgainstNullOrWhitespace(emailAddress, nameof(emailAddress));
        Guard.Against<DomainException> (!_regex.IsMatch(emailAddress), "Invalid email format.");

        Address = emailAddress.Trim().ToLowerInvariant();
    }

    public override string ToString() => Address;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Address;
    }
}
