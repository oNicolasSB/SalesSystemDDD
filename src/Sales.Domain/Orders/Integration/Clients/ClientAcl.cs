using Sales.Domain.Common.Exceptions;
using Sales.Domain.Common.Validation;

namespace Sales.Domain.Orders.Integration.Clients;

public sealed class ClientAcl
{
    private readonly IClientGateway _clientGateway;

    public ClientAcl(IClientGateway clientGateway)
    {
        Guard.AgainstNull(clientGateway, nameof(clientGateway));
        _clientGateway = clientGateway;
    }

    public async Task<DeliveryAddressSnapshot> GetDeliveryAddressSnapshotAsync(Guid clientId, Guid addressId, CancellationToken cancellationToken = default)
    {
        var addressDto = await _clientGateway.GetAddressAsync(clientId, addressId, cancellationToken)
            ?? throw new DomainException($"Address not found on clients context.");

        return new DeliveryAddressSnapshot(
            addressDto.Cep,
            addressDto.Street,
            addressDto.Number,
            addressDto.Neighborhood,
            addressDto.City,
            addressDto.State,
            addressDto.Country,
            addressDto.Complement
        );
    }
}
