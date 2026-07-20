using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class StoreConfiguration : IEntityTypeConfiguration<Store>
{
    public void Configure(EntityTypeBuilder<Store> builder)
    {
        builder.Property(e => e.Name).HasMaxLength(300);
        builder.Property(e => e.LogoImageUrl).HasMaxLength(2000);
        builder.Property(e => e.PhoneNumber).HasMaxLength(100);
        builder.Property(e => e.Email).HasMaxLength(200);
        builder.Property(e => e.FacebookLink).HasMaxLength(2000);
        builder.Property(e => e.InstagramLink).HasMaxLength(2000);
        builder.Property(e => e.CoverStoreImageLink).HasMaxLength(2000);
        builder.Property(e => e.Slug).HasMaxLength(500);
        builder.Property(e => e.NumberOfOrders).HasDefaultValue(0);
    }
}
