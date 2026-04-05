using Sales.Domain.Clients.Entities;
using Sales.Domain.Common.Exceptions;

namespace Sales.Domain.Tests.Clients.Entities;

public class AddressTest
{
    private static Address CreateValidAddress()
    {
        return new Address(
            cep: "12345678",
            street: "Main Street",
            number: "123",
            neighborhood: "Downtown",
            city: "City",
            state: "State",
            country: "Country",
            complement: "Apt 1"
        );
    }

    [Fact(DisplayName = "Create Address with valid data should succeed")]
    public void CreateAddress_WithValidData_ShouldSucceed()
    {
        // Act
        var address = CreateValidAddress();
        // Assert
        Assert.Equal("12345678", address.Cep);
        Assert.Equal("Main Street", address.Street);
        Assert.Equal("123", address.Number);
        Assert.Equal("Downtown", address.Neighborhood);
        Assert.Equal("City", address.City);
        Assert.Equal("State", address.State);
        Assert.Equal("Country", address.Country);
        Assert.Equal("Apt 1", address.Complement);
    }

    [Theory(DisplayName = "Create Address with invalid CEP should throw DomainException")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void CreateAddress_WithInvalidCep_ShouldThrowDomainException(string invalidCep)
    {
        // Act & Assert
        Action act = () => new Address(
            cep: invalidCep,
            street: "Main Street",
            number: "123",
            neighborhood: "Downtown",
            city: "City",
            state: "State",
            country: "Country"
        );

        act.Should().Throw<DomainException>()
            .WithMessage("'Cep' cannot be null or whitespace.");
    }

    [Fact]
    public void CreateAddress_WithInvalidCepFormat_ShouldThrowDomainException()
    {
        // Act & Assert
        Action act = () => new Address(
            cep: "1234",
            street: "Main Street",
            number: "123",
            neighborhood: "Downtown",
            city: "City",
            state: "State",
            country: "Country"
        );

        act.Should().Throw<DomainException>()
            .WithMessage("Invalid CEP format. It should contain exactly 8 digits.");
    }

    [Theory(DisplayName = "Update Address with valid data should succeed")]
    [InlineData(null, "Street", "123", "Downtown", "City", "State", "Country")]
    [InlineData("12345678", null, "123", "Downtown", "City", "State", "Country")]
    [InlineData("12345678", "St", null, "Downtown", "City", "State", "Country")]
    [InlineData("12345678", "Main Street", null, "Downtown", "City", "State", "Country")]
    [InlineData("12345678", "Main Street", "123", null, "City", "State", "Country")]
    [InlineData("12345678", "Main Street", "123", "Downtown", null, "State", "Country")]
    [InlineData("12345678", "Main Street", "123", "Downtown", "City", null, "Country")]
    [InlineData("12345678", "Main Street", "123", "Downtown", "City", "State", null)]
    public void CreateAddress_WithInvalidData_ShouldThrowDomainException(
        string cep,
        string street,
        string number,
        string neighborhood,
        string city,
        string state,
        string country
    )
    {
        // Act & Assert
        Action act = () => new Address(
            cep: cep,
            street: street,
            number: number,
            neighborhood: neighborhood,
            city: city,
            state: state,
            country: country
        );

        act.Should().Throw<DomainException>();
    }

    [Fact(DisplayName = "Update Address with valid data should succeed")]
    public void UpdateAddress_WithValidData_ShouldSucceed()
    {
        // Arrange
        var address = CreateValidAddress();
        // Act
        address.Update(
            cep: "87654321",
            street: "Second Street",
            number: "456",
            neighborhood: "Uptown",
            city: "New City",
            state: "New State",
            country: "New Country",
            complement: "Apt 2"
        );
        // Assert
        Assert.Equal("87654321", address.Cep);
        Assert.Equal("Second Street", address.Street);
        Assert.Equal("456", address.Number);
        Assert.Equal("Uptown", address.Neighborhood);
        Assert.Equal("New City", address.City);
        Assert.Equal("New State", address.State);
        Assert.Equal("New Country", address.Country);
        Assert.Equal("Apt 2", address.Complement);
    }

    [Fact]
    public void UpdateAddress_WithInvalidCep_ShouldThrowDomainException()
    {
        // Arrange
        var address = CreateValidAddress();
        // Act & Assert
        Action act = () => address.Update(
            cep: "123",
            street: "Second Street",
            number: "456",
            neighborhood: "Uptown",
            city: "New City",
            state: "New State",
            country: "New Country"
        );

        act.Should().Throw<DomainException>()
            .WithMessage("Invalid CEP format. It should contain exactly 8 digits.");
    }

    [Theory(DisplayName = "Update Address with invalid data should throw DomainException")]
    [InlineData(null, "Street", "123", "Downtown", "City", "State", "Country")]
    [InlineData("12345678", null, "123", "Downtown", "City", "State", "Country")]
    [InlineData("12345678", "St", null, "Downtown", "City", "State", "Country")]
    [InlineData("12345678", "Main Street", null, "Downtown", "City", "State", "Country")]
    [InlineData("12345678", "Main Street", "123", null, "City", "State", "Country")]
    [InlineData("12345678", "Main Street", "123", "Downtown", null, "State", "Country")]
    [InlineData("12345678", "Main Street", "123", "Downtown", "City", null, "Country")]
    [InlineData("12345678", "Main Street", "123", "Downtown", "City", "State", null)]
    public void UpdateAddress_WithInvalidData_ShouldThrowDomainException(
        string cep,
        string street,
        string number,
        string neighborhood,
        string city,
        string state,
        string country
    )
    {
        // Act & Assert
        Action act = () => CreateValidAddress().Update(
            cep: cep,
            street: street,
            number: number,
            neighborhood: neighborhood,
            city: city,
            state: state,
            country: country
        );

        act.Should().Throw<DomainException>();
    }
}
