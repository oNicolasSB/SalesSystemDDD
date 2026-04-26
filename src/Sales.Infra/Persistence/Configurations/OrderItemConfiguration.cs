using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sales.Domain.Orders.Entities;

namespace Sales.Infra.Persistence.Configurations;

public sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("order_items");
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).ValueGeneratedNever();

        builder.Property<Guid>("OrderId").IsRequired();
        builder.Property(i => i.UpdatedAt).IsRequired(false);
        builder.Ignore(i => i.DomainEvents);
        builder.Property(i => i.ProductName)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(i => i.UnitPrice).HasPrecision(18, 2).IsRequired();
        builder.Property(i => i.TotalPrice).HasPrecision(18, 2).IsRequired();
        builder.Property(i => i.AppliedDiscount).HasPrecision(18, 2).IsRequired();
        builder.Property(i => i.Quantity).IsRequired();
    }
}
