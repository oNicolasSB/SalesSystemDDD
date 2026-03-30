using Sales.Domain.Clients.Enums;
using Sales.Domain.Clients.ValueObjects;
using Sales.Domain.Common.Base;

namespace Sales.Domain.Clients.Entities;

public sealed class Cliente : AggregateRoot
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

    public Cliente(
        FullName name,
        Cpf cpf,
        Email email,
        PhoneNumber phoneNumber,
        ClientStatus clientStatus,
        Sex sex,
        MaritalStatus maritalStatus,
        Guid mainAddressId
    )
    {
        Id = id;
        Name = name;
        Cpf = cpf;
        Email = email;
        PhoneNumber = phoneNumber;
        ClientStatus = clientStatus;
        Sex = sex;
        MaritalStatus = maritalStatus;
        MainAddressId = mainAddressId;
    }

}
