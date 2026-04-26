namespace Sales.Api.Endpoints.Orders;

public record class CreateOrderRequest(Guid ClientId, Guid AddressId);