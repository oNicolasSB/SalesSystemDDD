using System.Text.RegularExpressions;
using Sales.Domain.Common.Base;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Common.Validation;

namespace Sales.Domain.Clients.Entities;

public sealed class Address : Entity
{
    public string Cep { get; private set; }
    public string Street { get; private set; }
    public string Number { get; private set; }
    public string Neighborhood { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string Country { get; private set; }
    public string Complement { get; private set; }

    public Address(
        string cep,
        string street,
        string number,
        string neighborhood,
        string city,
        string state,
        string country,
        string complement = ""
    )
    {
        Validate(cep, street, number, neighborhood, city, state, country);

        Cep = cep;
        Street = street;
        Number = number;
        Neighborhood = neighborhood;
        City = city;
        State = state;
        Country = country;
        Complement = complement;
    }

    internal void Update(
        string cep,
        string street,
        string number,
        string neighborhood,
        string city,
        string state,
        string country,
        string complement = ""
    )
    {
        Validate(cep, street, number, neighborhood, city, state, country);

        Cep = cep;
        Street = street;
        Number = number;
        Neighborhood = neighborhood;
        City = city;
        State = state;
        Country = country;
        Complement = complement;
    }

    private static void Validate(string cep, string street, string number, string neighborhood, string city, string state, string country)
    {
        Guard.AgainstNullOrWhitespace(cep, nameof(cep));
        Guard.Against<DomainException>(!Regex.IsMatch(cep, @"^\d{8}$"), "Invalid CEP format. It should contain exactly 8 digits.");
        Guard.AgainstNullOrWhitespace(street, nameof(street));
        Guard.Against<DomainException>(street.Length < 3, "Street must be at least 3 characters long.");
        Guard.AgainstNullOrWhitespace(number, nameof(number));
        Guard.AgainstNullOrWhitespace(neighborhood, nameof(neighborhood));
        Guard.AgainstNullOrWhitespace(city, nameof(city));
        Guard.AgainstNullOrWhitespace(state, nameof(state));
        Guard.AgainstNullOrWhitespace(country, nameof(country));
    }
}
