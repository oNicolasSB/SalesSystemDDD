using Sales.Domain.Common.Exceptions;
using Sales.Domain.Common.Validation;
using Sales.Domain.Orders.ValueObjects;

namespace Sales.Domain.Orders.Integration.Clients;

public sealed class ClientAcl
{
    public DeliveryAddress TranslateAddress(AddressDto addressDto)
    {

        return DeliveryAddress.Create(
            addressDto.Cep,
            addressDto.Street,
            addressDto.Number,
            addressDto.Complement,
            addressDto.Neighborhood,
            addressDto.City,
            addressDto.State,
            addressDto.Country
        );
    }
}
