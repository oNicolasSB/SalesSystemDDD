using System.Text.RegularExpressions;
using Sales.Domain.Base;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Validation;

namespace Sales.Domain.ValueObjects;

public class DeliveryAddress : ValueObject
{
    public string Cep { get; private set; } = string.Empty;
    public string Street { get; private set; } = string.Empty;
    public string Complement { get; private set; } = string.Empty;
    public string Neighborhood { get; private set; } = string.Empty;
    public string City { get; private set; } = string.Empty;
    public string State { get; private set; } = string.Empty;
    public string Country { get; private set; } = string.Empty;

    public static DeliveryAddress Create(string cep, string street, string complement, string neighborhood, string city, string state, string country)
    {
        return new DeliveryAddress(cep, street, complement, neighborhood, city, state, country);
    }

    private DeliveryAddress(string cep, string street, string complement, string neighborhood, string city, string state, string country)
    {
        Guard.AgainstNullOrWhitespace(cep, nameof(cep));
        Guard.AgainstNullOrWhitespace(street, nameof(street));
        Guard.AgainstNullOrWhitespace(neighborhood, nameof(neighborhood));
        Guard.AgainstNullOrWhitespace(city, nameof(city));
        Guard.AgainstNullOrWhitespace(state, nameof(state));
        Guard.AgainstNullOrWhitespace(country, nameof(country));

        if (!IsValidCep(cep))
        {
            throw new DomainException($"'{cep}' is not a valid CEP format.");
        }

        Cep = cep;
        Street = street;
        Complement = complement;
        Neighborhood = neighborhood;
        City = city;
        State = state;
        Country = country;
    }

    private static bool IsValidCep(string cep)
    {
        // Simple validation for Brazilian CEP format (5 digits + hyphen + 3 digits)
        return Regex.IsMatch(cep, @"^\d{5}-\d{3}$");
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Cep;
        yield return Street;
        yield return Complement;
        yield return Neighborhood;
        yield return City;
        yield return State;
        yield return Country;
    }

    public string FullAddress => $"{Street}, {Neighborhood}, {City} - {State}, {Country}, CEP: {Cep}";
}
