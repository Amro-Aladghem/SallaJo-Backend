using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class SellerRoleConfiguration : IEntityTypeConfiguration<SellerRole>
{
    public void Configure(EntityTypeBuilder<SellerRole> builder)
    {
        builder.Property(e => e.Name).HasMaxLength(200);
        builder.HasData(new[]
        {
            new SellerRole{Id=1,Name="Manager"},
            new SellerRole{Id=2,Name="Admin" }
        }
        );
    }
}
