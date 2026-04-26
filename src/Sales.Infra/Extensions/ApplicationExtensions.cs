using Microsoft.Extensions.DependencyInjection;
using Sales.Application.Commands.OrdersCommands.AddOrderItem;
using Sales.Application.Commands.OrdersCommands.CancelOrder;
using Sales.Application.Commands.OrdersCommands.CreateOrder;
using Sales.Application.Commands.OrdersCommands.MarkAsDelivered;
using Sales.Application.Commands.OrdersCommands.MarkAsInPreparation;
using Sales.Application.Commands.OrdersCommands.MarkAsShipped;
using Sales.Application.Commands.OrdersCommands.StartPayment;

namespace Sales.Infra.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
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
