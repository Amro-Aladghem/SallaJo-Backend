using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(e => e.Name).HasMaxLength(500);
        builder.Property(e => e.Price).HasColumnType("decimal(6,2)");
        builder.Property(e => e.PrimaryImageLink).HasMaxLength(2000);
        builder.Property(e => e.IsAcceptedToAppear).HasDefaultValue(true);
        builder.Property(e => e.NumberOfOrders).HasDefaultValue(0);
        builder.Property(e => e.IsDeleted).HasDefaultValue(false);
    }
}
