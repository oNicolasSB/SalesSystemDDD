using Sales.Domain.Clients.Enums;
using Sales.Domain.Clients.Events;
using Sales.Domain.Clients.ValueObjects;
using Sales.Domain.Common.Base;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Common.Validation;

namespace Sales.Domain.Clients.Entities;

public sealed class Client : AggregateRoot
{
    public FullName Name { get; private set; }
    public Cpf Cpf { get; private set; }
    public Email Email { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public ClientStatus ClientStatus { get; private set; }
    public Sex Sex { get; private set; }
    public MaritalStatus MaritalStatus { get; private set; }
    public Guid MainAddressId { get; private set; }

    public readonly List<Address> _addresses = [];
    public IReadOnlyCollection<Address> Addresses => _addresses.AsReadOnly();

    public Client(
        FullName name,
        Cpf cpf,
        Email email,
        PhoneNumber phoneNumber,
        Address mainAddress,
        Sex sex = Sex.PreferNotToSay,
        MaritalStatus maritalStatus = MaritalStatus.PreferNotToSay
    )
    {
        Validate(name, cpf, email, phoneNumber, mainAddress);
        Name = name;
        Cpf = cpf;
        Email = email;
        PhoneNumber = phoneNumber;
        ClientStatus = ClientStatus.Active;

        Sex = sex;
        MaritalStatus = maritalStatus;

        _addresses.Add(mainAddress);
        MainAddressId = mainAddress.Id;

        AddDomainEvent(new ClientRegisteredEvent(
            ClientId: Id,
            Name: Name.FormatedFullName,
            Cpf: Cpf.Number,
            Email: Email.Address
        ));
    }

    public void AddAddress(Address newAddress)
    {
        Guard.AgainstNull(newAddress, nameof(newAddress));
        _addresses.Add(newAddress);
        UpdateDate();
    }

    public void RemoveAddress(Guid addressId)
    {
        Address? address = _addresses.FirstOrDefault(a => a.Id == addressId);
        Guard.AgainstNull(address, nameof(address));

        _addresses.Remove(address!);

        if (addressId == MainAddressId)
        {
            MainAddressId = _addresses.FirstOrDefault()?.Id ?? Guid.Empty;
            AddDomainEvent(new MainAddressChangedEvent(
                ClientId: Id,
                NewAddressId: MainAddressId
            ));
        }
        UpdateDate();
    }

    public void UpdateAdress(Guid addressId, string cep, string street, string number, string neighborhood, string city, string state, string country, string complement = "")
    {
        Address? address = _addresses.FirstOrDefault(a => a.Id == addressId);
        Guard.AgainstNull(address, nameof(address));

        address!.Update(cep, street, number, neighborhood, city, state, country, complement);

        UpdateDate();
    }

    public void SetMainAdress(Guid addressId)
    {
        Address? address = _addresses.FirstOrDefault(a => a.Id == addressId);
        Guard.AgainstNull(address, nameof(address));

        MainAddressId = addressId;

        AddDomainEvent(new MainAddressChangedEvent(
            ClientId: Id,
            NewAddressId: MainAddressId
        ));

        UpdateDate();
    }

    public Address GetMainAddress()
    {
        return _addresses.First(a => a.Id == MainAddressId);
    }

    public void UpdateProfile(
        FullName name,
        Email email,
        PhoneNumber phoneNumber,
        Sex sex,
        MaritalStatus maritalStatus
    )
    {
        Guard.Against<DomainException>(ClientStatus == ClientStatus.Blocked, "Blocked clients cannot update their profile.");

        Guard.AgainstNull(name, nameof(name));
        Guard.AgainstNull(email, nameof(email));
        Guard.AgainstNull(phoneNumber, nameof(phoneNumber));

        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;

        Sex = sex;
        MaritalStatus = maritalStatus;
        UpdateDate();
    }

    public void Block()
    {
        Guard.Against<DomainException>(ClientStatus == ClientStatus.Blocked, "Client is already blocked.");
        ClientStatus = ClientStatus.Blocked;

        AddDomainEvent(new ClientBlockedEvent(ClientId: Id, Cpf: Cpf.Number));
        UpdateDate();
    }

    public void Activate()
    {
        Guard.Against<DomainException>(ClientStatus == ClientStatus.Active, "Client is already active.");
        ClientStatus = ClientStatus.Active;
        UpdateDate();
    }

    private static void Validate(FullName name, Cpf cpf, Email email, PhoneNumber phoneNumber, Address mainAddress)
    {
        Guard.AgainstNull(name, nameof(name));
        Guard.AgainstNull(cpf, nameof(cpf));
        Guard.AgainstNull(email, nameof(email));
        Guard.AgainstNull(phoneNumber, nameof(phoneNumber));
        Guard.AgainstNull(mainAddress, nameof(mainAddress));
    }

}
