using Microsoft.Extensions.DependencyInjection;
using Sales.Application.Abstractions.Persistence;
using Sales.Application.Commands.OrdersCommands.AddOrderItem;
using Sales.Application.Commands.OrdersCommands.CancelOrder;
using Sales.Application.Commands.OrdersCommands.CreateOrder;
using Sales.Application.Commands.OrdersCommands.MarkAsDelivered;
using Sales.Application.Commands.OrdersCommands.MarkAsInPreparation;
using Sales.Application.Commands.OrdersCommands.MarkAsShipped;
using Sales.Application.Commands.OrdersCommands.StartPayment;
using Sales.Domain.Orders.Integration.Catalog;
using Sales.Domain.Orders.Integration.Clients;

namespace Sales.Infra.Fakes;

public static class FakeInfrastructureExtension
{
    public static IServiceCollection AddFakeInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<FakeOrderRepository>();
        services.AddSingleton<IOrderRepository>(sp => sp.GetRequiredService<FakeOrderRepository>());

        services.AddSingleton<ICatalogGateway, FakeCatalogGateway>();
        services.AddSingleton<IClientGateway, FakeClientsGateway>();

        services.AddSingleton<CatalogAcl>();
        services.AddSingleton<ClientAcl>();

        services.AddScoped<CreateOrderCommandHandler>();
        services.AddScoped<AddOrderItemCommandHandler>();
        services.AddScoped<StartPaymentCommandHandler>();
        services.AddScoped<MarkAsShippedCommandHandler>();
        services.AddScoped<MarkAsDeliveredCommandHandler>();
        services.AddScoped<CancelOrderCommandHandler>();
        services.AddScoped<MarkAsInPreparationCommandHandler>();
        return services;

    }
}
