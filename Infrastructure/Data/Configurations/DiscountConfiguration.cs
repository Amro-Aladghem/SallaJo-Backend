using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class DiscountConfiguration : IEntityTypeConfiguration<Discount>
{
    public void Configure(EntityTypeBuilder<Discount> builder)
    {
        builder.Property(e => e.DiscountAmount).HasColumnType("decimal(6,2)");
        builder.Property(e => e.LeastAmountNumber).HasDefaultValue(1);
    }
}
