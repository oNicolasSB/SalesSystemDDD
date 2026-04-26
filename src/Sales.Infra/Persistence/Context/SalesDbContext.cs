using Microsoft.EntityFrameworkCore;
using Sales.Domain.Orders.Entities;

namespace Sales.Infra.Persistence.Context;

public class SalesDbContext : DbContext
{
    public SalesDbContext(DbContextOptions<SalesDbContext> options) : base(options) { }

    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SalesDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
