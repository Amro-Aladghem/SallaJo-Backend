using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class OfferConfiguration : IEntityTypeConfiguration<Offer>
{
    public void Configure(EntityTypeBuilder<Offer> builder)
    {
        builder.Property(e => e.ImageLink).HasMaxLength(2000);
        builder.Property(e => e.Title).HasMaxLength(300);
        builder.Property(e => e.OfferPrice).HasColumnType("decimal(6,2)");
    }
}
