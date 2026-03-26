using FluentAssertions;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.ValueObjects;

namespace Sales.Domain.Tests.ValueObjects;

public class DeliveryAddressTests
{
    [Fact(DisplayName = "Create should return DeliveryAddress when parameters are valid")]

    public void Create_ShouldReturnDeliveryAddress_WhenValidParameters()
    {
        // Arrange
        string cep = "12345-678";
        string street = "Street";
        string complement = "Complement";
        string neighborhood = "Neighborhood";
        string city = "City";
        string state = "State";
        string country = "Country";

        // Act
        DeliveryAddress deliveryAddress = DeliveryAddress.Create(cep, street, complement, neighborhood, city, state, country);

        // Assert
        deliveryAddress.Should().NotBeNull();
        deliveryAddress.Cep.Should().Be(cep);
        deliveryAddress.Street.Should().Be(street);
        deliveryAddress.Complement.Should().Be(complement);
        deliveryAddress.Neighborhood.Should().Be(neighborhood);
        deliveryAddress.City.Should().Be(city);
        deliveryAddress.State.Should().Be(state);
        deliveryAddress.Country.Should().Be(country);
        deliveryAddress.FullAddress.Should().Be($"{street}, {neighborhood}, {city} - {state}, {country}, CEP: {cep}");
    }

    [Theory(DisplayName = "Create should throw DomainException when CEP is invalid")]
    [InlineData("12345678")]
    [InlineData("1234-5678")]
    [InlineData("12-345678")]
    [InlineData("ABCDE-FGH")]
    public void Create_ShouldThrowDomainException_WhenInvalidCep(string invalidCep)
    {
        // Arrange
        string street = "Street";
        string complement = "Complement";
        string neighborhood = "Neighborhood";
        string city = "City";
        string state = "State";
        string country = "Country";

        // Act
        Action act = () => DeliveryAddress.Create(invalidCep, street, complement, neighborhood, city, state, country);

        // Assert
        act.Should().Throw<DomainException>().WithMessage($"'{invalidCep}' is not a valid CEP format.");
    }

    [Fact(DisplayName = "Two DeliveryAddress instances with the same values should be equal")]
    public void Equals_ShouldReturnTrue_ForSameValues()
    {
        // Arrange
        var address1 = DeliveryAddress.Create("12345-678", "Street", "Complement", "Neighborhood", "City", "State", "Country");
        var address2 = DeliveryAddress.Create("12345-678", "Street", "Complement", "Neighborhood", "City", "State", "Country");

        // Act & Assert
        address1.Should().Be(address2);
    }

    [Fact(DisplayName = "Two DeliveryAddress instances with different values should not be equal")]
    public void Equals_ShouldReturnFalse_ForDifferentValues()
    {
        // Arrange
        var address1 = DeliveryAddress.Create("12345-678", "Street", "Complement", "Neighborhood", "City", "State", "Country");
        var address2 = DeliveryAddress.Create("87654-321", "Another Street", "Another Complement", "Another Neighborhood", "Another City", "Another State", "Another Country");

        // Act & Assert
        address1.Should().NotBe(address2);
    }

    [Fact(DisplayName = "DeliveryAddress should be immutable")]
    public void DeliveryAddress_ShouldBeImmutable()
    {
        // Arrange
        var address = DeliveryAddress.Create("12345-678", "Street", "Complement", "Neighborhood", "City", "State", "Country");

        // Act
        Action act = () =>
        {
            // address.Cep = "87654-321";
        };

        // Assert
        address.GetType().GetProperties().All(p => p.SetMethod == null)
            .Should().BeTrue("All properties should be read-only to ensure immutability.");
    }

    [Theory(DisplayName = "Should throw a DomainException when required fields are null or whitespace")]
    [InlineData(null, "Street", "Complement", "Neighborhood", "City", "State", "Country", "cep")]
    [InlineData("12345-678", null, "Complement", "Neighborhood", "City", "State", "Country", "street")]
    [InlineData("12345-678", "Street", "Complement", "Neighborhood", "City", "State", null, "country")]
    public void Create_ShouldThrowDomainException_WhenRequiredFieldsAreNullOrWhitespace(string cep, string street, string complement, string neighborhood, string city, string state, string country, string parameterName)
    {
        // Act
        Action act = () => DeliveryAddress.Create(cep, street, complement, neighborhood, city, state, country);

        // Assert
        act.Should().Throw<DomainException>().WithMessage($"'{parameterName}' cannot be null or whitespace.");
    }
}
