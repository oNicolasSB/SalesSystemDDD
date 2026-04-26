using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sales.Domain.Orders.Entities;

namespace Sales.Infra.Persistence.Configurations;

public sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("payments");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedNever();

        builder.Property(p => p.UpdatedAt).IsRequired(false);
        builder.Ignore(p => p.DomainEvents);
        builder.Property(p => p.Value).HasPrecision(18, 2).IsRequired();
        builder.Property(p => p.PaymentMethod)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(p => p.PaymentStatus)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(p => p.TransactionCode).HasMaxLength(100);
    }
}
