using Sales.Application.Abstractions.Persistence;
using Sales.Application.Commands.OrdersCommands.AddOrderItem;
using Sales.Application.Commands.OrdersCommands.CancelOrder;
using Sales.Application.Commands.OrdersCommands.CreateOrder;
using Sales.Application.Commands.OrdersCommands.MarkAsDelivered;
using Sales.Application.Commands.OrdersCommands.MarkAsInPreparation;
using Sales.Application.Commands.OrdersCommands.MarkAsShipped;
using Sales.Application.Commands.OrdersCommands.StartPayment;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Orders.Enums;

namespace Sales.Api.Endpoints.Orders;

public static class OrdersEndpoints
{
    public static WebApplication MapOrdersEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/orders")
            .WithTags("Orders");

        group.MapGet("/fake-ids", () => Results.Ok(new
        {
            clients = new[]
            {
                new
                {
                    clientId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    addresses = new[]
                    {
                        new { addressId = Guid.Parse("11111111-1111-1111-1111-111111111111"), description = "Street 1, 123, Neighborhood 1, State 1" },
                        new { addressId = Guid.Parse("66666666-6666-6666-6666-666666666666"), description = "Street 6, 678, Neighborhood 6, State 6" }
                    }
                },
                new
                {
                    clientId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    addresses = new[]
                    {
                        new { addressId = Guid.Parse("22222222-2222-2222-2222-222222222222"), description = "Street 2, 456, Neighborhood 2, State 2" }
                    }
                },
                new
                {
                    clientId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    addresses = new[]
                    {
                        new { addressId = Guid.Parse("33333333-3333-3333-3333-333333333333"), description = "Street 3, 789, Neighborhood 3, State 3" }
                    }
                },
                new
                {
                    clientId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                    addresses = new[]
                    {
                        new { addressId = Guid.Parse("44444444-4444-4444-4444-444444444444"), description = "Street 4, 012, Neighborhood 4, State 4" }
                    }
                },
                new
                {
                    clientId = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                    addresses = new[]
                    {
                        new { addressId = Guid.Parse("55555555-5555-5555-5555-555555555555"), description = "Street 5, 345, Neighborhood 5, State 5" }
                    }
                }
            },
            products = new[]
            {
                new { productId = Guid.Parse("11111111-1111-1111-1111-111111111111"), description = "Product 1 - R$ 10,00" },
                new { productId = Guid.Parse("22222222-2222-2222-2222-222222222222"), description = "Product 2 - R$ 20,00" },
                new { productId = Guid.Parse("33333333-3333-3333-3333-333333333333"), description = "Product 3 - R$ 30,00" },
                new { productId = Guid.Parse("44444444-4444-4444-4444-444444444444"), description = "Product 4 - R$ 40,00" },
                new { productId = Guid.Parse("55555555-5555-5555-5555-555555555555"), description = "Product 5 - R$ 50,00" }
            }
        })).WithSummary("Get fake IDs for testing purposes");

        // group.MapGet("/", async (
        //     IOrderRepository repository,
        //     CancellationToken cancellationToken) =>
        // {
        //     var orders = await repository.ListAllAsync(cancellationToken);
        //     var result = orders.Select(o => new
        //     {
        //         o.Id,
        //         o.OrderNumber,
        //         o.ClientId,
        //         o.TotalValue,
        //         Status = o.OrderStatus.ToString(),
        //         o.CreatedAt,
        //         TotalItens = o.OrderItems.Count
        //     });
        //     return Results.Ok(result);
        // }).WithSummary("Get all orders");

        group.MapGet("/{id:guid}", async (
            Guid id,
            IOrderRepository repository,
            CancellationToken cancellationToken) =>
        {
            var order = await repository.GetByIdAsync(id, cancellationToken);
            if (order is null)
            {
                return Results.NotFound();
            }
            var result = new
            {
                order.Id,
                order.OrderNumber,
                order.ClientId,
                order.TotalValue,
                Status = order.OrderStatus.ToString(),
                order.CreatedAt,
                order.UpdatedAt,
                Address = new
                {
                    order.DeliveryAddress.Street,
                    order.DeliveryAddress.Number,
                    order.DeliveryAddress.Neighborhood,
                    order.DeliveryAddress.State,
                    order.DeliveryAddress.City

                },
                Items = order.OrderItems.Select(oi => new
                {
                    oi.Id,
                    oi.ProductId,
                    oi.ProductName,
                    oi.Quantity,
                    oi.UnitPrice,
                    oi.TotalPrice
                }),
                payments = order.Payments.Select(p => new
                {
                    p.Id,
                    Method = p.PaymentMethod.ToString(),
                    Status = p.PaymentStatus.ToString(),
                    p.Value,
                    p.TransactionCode,
                    p.PaidAt
                })
            };
            return Results.Ok(result);
        }).WithSummary("Get an order by ID");

        group.MapPost("/", async (
            CreateOrderRequest request,
            CreateOrderCommandHandler handler,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var command = new CreateOrderCommand(request.ClientId, request.AddressId);
                var result = await handler.HandleAsync(command, cancellationToken);
                return Results.Created($"/orders/{result.OrderId}", result);

            }
            catch (InvalidOperationException ex)
            {
                return Results.NotFound(new { error = ex.Message });
            }
            catch (DomainException ex)
            {
                return Results.UnprocessableEntity(new { error = ex.Message });
            }
        }).WithSummary("Create a new order");


        group.MapPost("/{id:guid}/items", async (
            Guid id,
            AddOrderItemRequest request,
            AddOrderItemCommandHandler handler,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var command = new AddOrderItemCommand(id, request.ProductId, request.Quantity);
                var result = await handler.HandleAsync(command, cancellationToken);
                return Results.Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return Results.NotFound(new { error = ex.Message });
            }
            catch (DomainException ex)
            {
                return Results.UnprocessableEntity(new { error = ex.Message });
            }
        }).WithSummary("Add an item to an existing order");

        group.MapPost("{id:guid}/payment", async (
            Guid id,
            StartPaymentRequest request,
            StartPaymentCommandHandler handler,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var method = (PaymentMethod)request.PaymentMethod;
                var command = new StartPaymentCommand(id, method);
                var result = await handler.HandleAsync(command, cancellationToken);
                return Results.Ok(result);
            }
            catch (DomainException ex)
            {
                return Results.UnprocessableEntity(new { error = ex.Message });
            }
        }).WithSummary("Start payment for an order");

        group.MapPost("/{id:guid}/payment/confirmation", async (
            Guid id,
            ConfirmPaymentRequest request,
            IOrderRepository repository,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var order = await repository.GetByIdAsync(id, cancellationToken);
                if (order is null)
                {
                    return Results.NotFound(new { error = $"Order with id {id} not found." });
                }

                var payment = order.Payments.FirstOrDefault(p => p.Id == request.PaymentId);
                if (payment is null)
                {
                    return Results.NotFound(new { error = $"Payment not found." });
                }

                payment.GenerateLocalTransactionCode();

                payment.ConfirmPayment();

                order.HandlePaymentApproved(payment.Id);

                await repository.UpdateAsync(order, cancellationToken);

                var result = new
                {
                    OrderId = order.Id,
                    PaymentId = payment.Id,
                    OrderStatus = order.OrderStatus.ToString(),
                    PaymentStatus = payment.PaymentStatus.ToString(),
                    TransactionCode = payment.TransactionCode
                };

                return Results.Ok(result);
            }
            catch (DomainException ex)
            {
                return Results.UnprocessableEntity(new { error = ex.Message });
            }
        }).WithSummary("Confirm payment for an order")
        .WithDescription("SIMULATION - In a real scenario, the payment confirmation would be done by the payment gateway through a webhook or similar mechanism. This endpoint is just for testing purposes.");

        group.MapPost("/{id:guid}/preparation", async (
            Guid id,
            MarkAsInPreparationCommandHandler handler,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var command = new MarkAsInPreparationCommand(id);
                var result = await handler.HandleAsync(command, cancellationToken);

                return Results.Ok(result);
            }
            catch (DomainException ex)
            {
                return Results.UnprocessableEntity(new { error = ex.Message });
            }
        }).WithSummary("Mark an order as in preparation");

        group.MapPost("/{id:guid}/sent", async (
            Guid id,
            MarkAsShippedCommandHandler handler,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var command = new MarkAsShippedCommand(id);
                var result = await handler.HandleAsync(command, cancellationToken);

                return Results.Ok(result);
            }
            catch (DomainException ex)
            {
                return Results.UnprocessableEntity(new { error = ex.Message });
            }
        }).WithSummary("Mark an order as shipped");

        group.MapPost("/{id:guid}/delivered", async (
            Guid id,
            MarkAsDeliveredCommandHandler handler,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var command = new MarkAsDeliveredCommand(id);
                var result = await handler.HandleAsync(command, cancellationToken);

                return Results.Ok(result);
            }
            catch (DomainException ex)
            {
                return Results.UnprocessableEntity(new { error = ex.Message });
            }
        }).WithSummary("Mark an order as delivered");

        group.MapPost("/{id:guid}/cancel", async (
            Guid id,
            CancelOrderRequest request,
            CancelOrderCommandHandler handler,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var command = new CancelOrderCommand(id, request.Reason ?? "OTHER");
                var result = await handler.HandleAsync(command, cancellationToken);

                return Results.Ok(result);
            }
            catch (DomainException ex)
            {
                return Results.UnprocessableEntity(new { error = ex.Message });
            }
        }).WithSummary("Cancel an order")
        .WithDescription("The cancellation reason is optional, but if provided, it will be recorded in the order history. Valid reason codes are: \n" +
            "CUST_REQUEST\n" +
            "PAYMENT_ISSUE\n" +
            "OUT_OF_STOCK\n" +
            "INVALID_ADDRESS\n" +
            "OTHER\n");

        return app;
    }
}
