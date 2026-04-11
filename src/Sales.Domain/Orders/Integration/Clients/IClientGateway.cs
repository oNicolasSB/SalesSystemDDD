namespace Sales.Domain.Orders.Integration.Clients;

public interface IClientGateway
{
    Task<AddressDto?> GetAddressAsync(Guid clientId, Guid addressId, CancellationToken cancellationToken = default);
}
