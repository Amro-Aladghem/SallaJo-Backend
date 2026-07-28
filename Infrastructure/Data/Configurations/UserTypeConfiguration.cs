using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class UserTypeConfiguration : IEntityTypeConfiguration<UserType>
{
    public void Configure(EntityTypeBuilder<UserType> builder)
    {
        builder.Property(e => e.Name).HasMaxLength(200);
        builder.HasData(new[]
        {
            new { Id = 1,Name="Person" },
            new {Id=2,Name="Seller"},
            new {Id=3,Name="Admin"}
        } );
    }
}
