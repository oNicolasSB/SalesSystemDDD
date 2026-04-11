namespace Sales.Domain.Orders.Integration.Clients;

public class AddressDto
{
    public Guid Id { get; }
    public string Cep { get; }
    public string Street { get; }
    public string Number { get; }
    public string Neighborhood { get; }
    public string City { get; }
    public string State { get; }
    public string Country { get; }
    public string Complement { get; }
    public AddressDto(Guid id, string cep, string street, string number, string neighborhood, string city, string state, string country, string complement)
    {
        Id = id;
        Cep = cep;
        Street = street;
        Number = number;
        Neighborhood = neighborhood;
        City = city;
        State = state;
        Country = country;
        Complement = complement ?? string.Empty;
    }
}
