using Sales.Domain.Orders.Integration.Clients;

namespace Sales.Infra.Fakes;

public sealed class FakeClientsGateway : IClientGateway
{
    private static readonly Dictionary<Guid, Dictionary<Guid, AddressDto>> _clients = new()
    {
        [Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")] = new Dictionary<Guid, AddressDto>
        {
            [Guid.Parse("11111111-1111-1111-1111-111111111111")] = new AddressDto(new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "29300000", "Street 1", "123", "Neighborhood 1", "City 1", "State 1", "Country 1", "Complement 1"),
            [Guid.Parse("66666666-6666-6666-6666-666666666666")] = new AddressDto(new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "29300000", "Street 6", "678", "Neighborhood 6", "City 6", "State 6", "Country 6", "Complement 6")
        },
        [Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb")] = new Dictionary<Guid, AddressDto>
        {
            [Guid.Parse("22222222-2222-2222-2222-222222222222")] = new AddressDto(new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "29300000", "Street 2", "456", "Neighborhood 2", "City 2", "State 2", "Country 2", "Complement 2")
        },
        [Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc")] = new Dictionary<Guid, AddressDto>
        {
            [Guid.Parse("33333333-3333-3333-3333-333333333333")] = new AddressDto(new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "29300000", "Street 3", "789", "Neighborhood 3", "City 3", "State 3", "Country 3", "Complement 3")
        },
        [Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd")] = new Dictionary<Guid, AddressDto>
        {
            [Guid.Parse("44444444-4444-4444-4444-444444444444")] = new AddressDto(new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), "29300000", "Street 4", "012", "Neighborhood 4", "City 4", "State 4", "Country 4", "Complement 4")
        },
        [Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee")] = new Dictionary<Guid, AddressDto>
        {
            [Guid.Parse("55555555-5555-5555-5555-555555555555")] = new AddressDto(new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), "29300000", "Street 5", "345", "Neighborhood 5", "City 5", "State 5", "Country 5", "Complement 5")
        }
    };
    public Task<AddressDto?> GetAddressAsync(Guid clientId, Guid addressId, CancellationToken cancellationToken = default)
    {
        if(_clients.TryGetValue(clientId, out var addresses) && addresses.TryGetValue(addressId, out var address))
        {
            return Task.FromResult<AddressDto?>(address);
        }
        return Task.FromResult<AddressDto?>(null);
    }
}
