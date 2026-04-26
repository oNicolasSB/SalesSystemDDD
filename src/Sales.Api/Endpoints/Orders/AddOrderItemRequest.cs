namespace Sales.Api.Endpoints.Orders;

public record AddOrderItemRequest(Guid ProductId, int Quantity);