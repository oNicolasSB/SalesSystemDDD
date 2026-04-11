using Sales.Domain.Common.Base;
using Sales.Domain.Common.Validation;

namespace Sales.Domain.Orders.Integration.Clients;

public sealed class DeliveryAddressSnapshot : ValueObject
{
    public string Cep { get; }
    public string Street { get; }
    public string Number { get; }
    public string Neighborhood { get; }
    public string City { get; }
    public string State { get; }
    public string Country { get; }
    public string Complement { get; }

    public DeliveryAddressSnapshot(string cep, string street, string number, string neighborhood, string city, string state, string country, string complement)
    {
        Guard.AgainstNullOrWhitespace(cep, nameof(cep));
        Guard.AgainstNullOrWhitespace(street, nameof(street));
        Guard.AgainstNullOrWhitespace(number, nameof(number));
        Guard.AgainstNullOrWhitespace(neighborhood, nameof(neighborhood));
        Guard.AgainstNullOrWhitespace(city, nameof(city));
        Guard.AgainstNullOrWhitespace(state, nameof(state));
        Guard.AgainstNullOrWhitespace(country, nameof(country));

        Cep = cep;
        Street = street;
        Number = number;
        Neighborhood = neighborhood;
        City = city;
        State = state;
        Country = country;
        Complement = complement ?? string.Empty;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Cep;
        yield return Street;
        yield return Number;
        yield return Neighborhood;
        yield return City;
        yield return State;
        yield return Country;
        yield return Complement;
    }
}
