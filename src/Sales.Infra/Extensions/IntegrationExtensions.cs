using Microsoft.Extensions.DependencyInjection;
using Sales.Domain.Orders.Integration.Catalog;
using Sales.Domain.Orders.Integration.Clients;
using Sales.Infra.Fakes;

namespace Sales.Infra.Extensions;

public static class IntegrationExtensions
{
    public static IServiceCollection AddFakeIntegrations(this IServiceCollection services)
    {
        services.AddSingleton<ICatalogGateway, FakeCatalogGateway>();
        services.AddSingleton<IClientGateway, FakeClientsGateway>();

        services.AddSingleton<CatalogAcl>();
        services.AddSingleton<ClientAcl>();

        return services;
    }
}
