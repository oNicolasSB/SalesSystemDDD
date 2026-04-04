using Sales.Domain.Clients.Entities;
using Sales.Domain.Clients.Enums;
using Sales.Domain.Clients.Events;
using Sales.Domain.Clients.ValueObjects;
using Sales.Domain.Common.Exceptions;

namespace Sales.Domain.Tests.Clients.Entities;

public class ClientTest
{
    private static FullName CreateFullName(string name = "John Doe") => new(name);
    private static Cpf CreateCpf(string number = "95145660090") => new(number);
    private static Email CreateEmail(string address = "john.doe@example.com") => new(address);
    private static PhoneNumber CreatePhoneNumber(string number = "11987654321") => new(number);
    private static Address CreateAddress(
        string street = "Main Street",
        string number = "123",
        string neighborhood = "Downtown",
        string city = "Anytown",
        string state = "State",
        string cep = "12345678",
        string country = "Country",
        string complement = "Apt 101") => new(cep, street, number, neighborhood, city, state, country, complement);

    private static Client CreateValidClient() => new(
        CreateFullName(),
        CreateCpf(),
        CreateEmail(),
        CreatePhoneNumber(),
        CreateAddress(),
        Sex.Male,
        MaritalStatus.Single
    );

    [Fact]
    public void CreateClient_WithValidData_ShouldSucceed()
    {
        // Arrange
        Client client = CreateValidClient();
        Address mainAddress = client.GetMainAddress();

        // Assert
        client.Should().NotBeNull();
        client.Name.FormatedFullName.Should().Be("John Doe");
        client.Cpf.Number.Should().Be("95145660090");
        client.Email.Address.Should().Be("john.doe@example.com");
        client.PhoneNumber.Number.Should().Be("11987654321");
        client.Addresses.Should().ContainSingle();
        mainAddress.Street.Should().Be("Main Street");
        mainAddress.Number.Should().Be("123");
        mainAddress.City.Should().Be("Anytown");
        mainAddress.State.Should().Be("State");
        mainAddress.Cep.Should().Be("12345678");
        mainAddress.Country.Should().Be("Country");
        mainAddress.Complement.Should().Be("Apt 101");
        client.Sex.Should().Be(Sex.Male);
        client.MaritalStatus.Should().Be(MaritalStatus.Single);
        client.ClientStatus.Should().Be(ClientStatus.Active);
    }

    [Fact]
    public void Constructor_ShouldAddClientRegisteredEvent()
    {
        // Arrange
        Client client = CreateValidClient();

        // Assert
        client.DomainEvents.Should().ContainSingle(e => e is ClientRegisteredEvent);
    }

    [Theory]
    [InlineData("Name")]
    [InlineData("Cpf")]
    [InlineData("Email")]
    [InlineData("PhoneNumber")]
    [InlineData("Address")]
    public void Constructor_WithNullOrInvalidData_ShouldThrowDomainException(string parameterName)
    {
        // Arrange
        FullName? fullName = parameterName == "Name" ? null : CreateFullName();
        Cpf? cpf = parameterName == "Cpf" ? null : CreateCpf();
        Email? email = parameterName == "Email" ? null : CreateEmail();
        PhoneNumber? phoneNumber = parameterName == "PhoneNumber" ? null : CreatePhoneNumber();
        Address? address = parameterName == "Address" ? null : CreateAddress();

        // Act
        Action act = () => new Client(
            fullName!,
            cpf!,
            email!,
            phoneNumber!,
            address!
        );

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void AddAddress_ShouldAddNewAddress()
    {
        // Arrange
        Client client = CreateValidClient();
        Address newAddress = CreateAddress(
            street: "Second Street",
            number: "456",
            neighborhood: "Uptown",
            city: "Othertown",
            state: "OtherState",
            cep: "87654321",
            country: "OtherCountry",
            complement: "Suite 202"
        );

        // Act
        client.AddAddress(newAddress);

        // Assert
        client.Addresses.Should().HaveCount(2);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void AddAddress_WithNullAddress_ShouldThrowDomainException(bool isNull)
    {
        // Arrange
        Client client = CreateValidClient();
        Address? newAddress = isNull ? null : CreateAddress();

        // Act
        Action act = () => client.AddAddress(newAddress!);

        // Assert
        if (isNull)
        {
            act.Should().Throw<DomainException>();
        }
        else
        {
            act.Should().NotThrow();
        }
    }

    [Fact]
    public void AddAddress_ShouldUpdateDate()
    {
        // Arrange
        Client client = CreateValidClient();
        DateTime initialUpdateDate = client.UpdatedAt ?? DateTime.UtcNow;
        Address newAddress = CreateAddress(
            street: "Second Street",
            number: "456",
            neighborhood: "Uptown",
            city: "Othertown",
            state: "OtherState",
            cep: "87654321",
            country: "OtherCountry",
            complement: "Suite 202"
        );

        Thread.Sleep(2); // Ensure UpdatedAt will be different after adding address

        // Act
        client.AddAddress(newAddress);

        // Assert
        client.UpdatedAt.Should().BeAfter(initialUpdateDate);
    }

    [Theory]
    [InlineData("NotExists")]
    [InlineData("Last")]
    public void RemoveAddress_ShouldThrowDomainException_WhenAddressDoesNotExist(string addressIdType)
    {
        // Arrange
        Client client = CreateValidClient();
        Guid addressId = addressIdType == "NotExists" ? Guid.NewGuid() : client.MainAddressId;

        // Act
        Action act = () => client.RemoveAddress(addressId);

        // Assert
        if (addressIdType == "NotExists")
        {
            act.Should().Throw<DomainException>();
        }
        else
        {
            act.Should().NotThrow();
        }
    }

    [Fact]
    public void RemoveAddress_ShouldUpdateMainAddress_WhenMainAddressIsRemoved()
    {
        // Arrange
        Client client = CreateValidClient();
        Guid mainAddressId = client.MainAddressId;
        Address newAddress = CreateAddress(
            street: "Second Street",
            number: "456",
            neighborhood: "Uptown",
            city: "Othertown",
            state: "OtherState",
            cep: "87654321",
            country: "OtherCountry",
            complement: "Suite 202"
        );
        client.AddAddress(newAddress);

        // Act
        client.RemoveAddress(mainAddressId);

        // Assert
        client.MainAddressId.Should().NotBe(mainAddressId);
        client.Addresses.Should().NotContain(a => a.Id == mainAddressId);
        client.Addresses.Should().ContainSingle(a => a.Id == newAddress.Id);
        client.DomainEvents.Should().ContainSingle(e => e is MainAddressChangedEvent && ((MainAddressChangedEvent)e).NewAddressId == newAddress.Id);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void UpdateAddress_ShouldThrowDomainException_WhenAddressDoesNotExist(bool isNull)
    {
        // Arrange
        Client client = CreateValidClient();
        Guid addressId = isNull ? Guid.NewGuid() : client.MainAddressId;

        // Act
        Action act = () => client.UpdateAdress(
            addressId,
            cep: "87654321",
            street: "Updated Street",
            number: "789",
            neighborhood: "Updated Neighborhood",
            city: "Updated City",
            state: "Updated State",
            country: "Updated Country",
            complement: "Updated Complement"
         );

        // Assert
        if (isNull)
        {
            act.Should().Throw<DomainException>();
        }
        else
        {
            act.Should().NotThrow();
        }
    }

    [Fact]
    public void UpdateAddress_ShouldUpdateAddress()
    {
        // Arrange
        Client client = CreateValidClient();
        Guid addressId = client.MainAddressId;

        // Act
        client.UpdateAdress(
            addressId,
            cep: "87654321",
            street: "Updated Street",
            number: "789",
            neighborhood: "Updated Neighborhood",
            city: "Updated City",
            state: "Updated State",
            country: "Updated Country",
            complement: "Updated Complement"
         );

        // Assert
        Address updatedAddress = client.Addresses.First(a => a.Id == addressId);
        updatedAddress.Street.Should().Be("Updated Street");
        updatedAddress.Number.Should().Be("789");
        updatedAddress.Neighborhood.Should().Be("Updated Neighborhood");
        updatedAddress.City.Should().Be("Updated City");
        updatedAddress.State.Should().Be("Updated State");
        updatedAddress.Cep.Should().Be("87654321");
        updatedAddress.Country.Should().Be("Updated Country");
        updatedAddress.Complement.Should().Be("Updated Complement");
    }

    [Fact]
    public void SetMainAddress_ShouldUpdateMainAddress()
    {
        // Arrange
        Client client = CreateValidClient();
        Address newAddress = CreateAddress(
            street: "Second Street",
            number: "456",
            neighborhood: "Uptown",
            city: "Othertown",
            state: "OtherState",
            cep: "87654321",
            country: "OtherCountry",
            complement: "Suite 202"
        );
        client.AddAddress(newAddress);

        // Act
        client.SetMainAdress(newAddress.Id);

        // Assert
        client.MainAddressId.Should().Be(newAddress.Id);
        client.DomainEvents.Should().ContainSingle(e => e is MainAddressChangedEvent && ((MainAddressChangedEvent)e).NewAddressId == newAddress.Id);
    }

    [Fact]
    public void GetMainAddress_ShouldReturnMainAddress()
    {
        // Arrange
        Client client = CreateValidClient();
        Address mainAddress = client.GetMainAddress();

        // Act
        Address retrievedMainAddress = client.GetMainAddress();

        // Assert
        retrievedMainAddress.Should().NotBeNull();
        retrievedMainAddress.Id.Should().Be(mainAddress.Id);
    }

    [Fact]
    public void UpdateProfile_ShouldUpdateClientProfile()
    {
        // Arrange
        Client client = CreateValidClient();
        FullName name = CreateFullName("Jane Doe");
        PhoneNumber phoneNumber = CreatePhoneNumber("11912345678");
        Email email = CreateEmail("jane.doe@example.com");

        // Act
        client.UpdateProfile(
            name,
            email,
            phoneNumber,
            Sex.Female,
            MaritalStatus.Married
        );

        // Assert
        client.Name.Should().Be(name);
        client.Email.Should().Be(email);
        client.PhoneNumber.Should().Be(phoneNumber);
        client.Sex.Should().Be(Sex.Female);
        client.MaritalStatus.Should().Be(MaritalStatus.Married);
    }

    [Theory]
    [InlineData("Name")]
    [InlineData("Email")]
    [InlineData("PhoneNumber")]
    public void UpdateProfile_WithNullData_ShouldThrowDomainException(string parameterName)
    {
        // Arrange
        Client client = CreateValidClient();
        FullName? name = parameterName == "Name" ? null : CreateFullName("Jane Doe");
        Email? email = parameterName == "Email" ? null : CreateEmail("jane.doe@example.com");
        PhoneNumber? phoneNumber = parameterName == "PhoneNumber" ? null : CreatePhoneNumber("11912345678");

        // Act
        Action act = () => client.UpdateProfile(
            name!,
            email!,
            phoneNumber!,
            Sex.Female,
            MaritalStatus.Married
        );

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void UpdateProfile_WhenClientIsBlocked_ShouldThrowDomainException()
    {
        // Arrange
        Client client = CreateValidClient();
        client.Block();
        FullName name = CreateFullName("Jane Doe");
        PhoneNumber phoneNumber = CreatePhoneNumber("11912345678");
        Email email = CreateEmail("jane.doe@example.com");


        // Act
        Action act = () => client.UpdateProfile(
            name,
            email,
            phoneNumber,
            Sex.Female,
            MaritalStatus.Married
        );

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Block_ShouldChangeClientStatusToBlocked()
    {
        // Arrange
        Client client = CreateValidClient();

        // Act
        client.Block();

        // Assert
        client.ClientStatus.Should().Be(ClientStatus.Blocked);
    }

    [Fact]
    public void Block_ShouldThrowEvent()
    {
        // Arrange
        Client client = CreateValidClient();

        // Act
        client.Block();

        // Assert
        client.DomainEvents.Should().ContainSingle(e => e is ClientBlockedEvent && ((ClientBlockedEvent)e).Cpf == client.Cpf.Number);
    }

    [Fact]
    public void Activate_ShouldChangeClientStatusToActive()
    {
        // Arrange
        Client client = CreateValidClient();
        client.Block();

        // Act
        client.Activate();

        // Assert
        client.ClientStatus.Should().Be(ClientStatus.Active);
    }

    [Fact]
    public void CompleteFlow_ShouldUpdateAllPropertiesCorrectly()
    {
        // Arrange
        Client client = CreateValidClient();
        Address newAddress = CreateAddress(
            street: "Second Street",
            number: "456",
            neighborhood: "Uptown",
            city: "Othertown",
            state: "OtherState",
            cep: "87654321",
            country: "OtherCountry",
            complement: "Suite 202"
        );
        client.AddAddress(newAddress);

        // Act
        client.SetMainAdress(newAddress.Id);
        client.UpdateProfile(
            CreateFullName("Jane Doe"),
            CreateEmail("jane.doe@example.com"),
            CreatePhoneNumber("11912345678"),
            Sex.Female,
            MaritalStatus.Married
        );

        // Assert
        client.Name.Should().Be(CreateFullName("Jane Doe"));
        client.Email.Should().Be(CreateEmail("jane.doe@example.com"));
        client.PhoneNumber.Should().Be(CreatePhoneNumber("11912345678"));
        client.Sex.Should().Be(Sex.Female);
        client.MaritalStatus.Should().Be(MaritalStatus.Married);
        client.MainAddressId.Should().Be(newAddress.Id);
        client.Addresses.Should().ContainSingle(a => a.Id == newAddress.Id);
        client.DomainEvents.Should().ContainSingle(e => e is MainAddressChangedEvent && ((MainAddressChangedEvent)e).NewAddressId == newAddress.Id);
        client.DomainEvents.Should().ContainSingle(e => e is ClientRegisteredEvent);
    }
}
