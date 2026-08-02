using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Configurations
{
    public class ContactTypeConfiguration : IEntityTypeConfiguration<ContactType>
    {
        public void Configure(EntityTypeBuilder<ContactType> builder)
        {
            builder.HasData(new[]
            {
                new ContactType(){Id=1,Name="whatsapp"},
                new ContactType(){Id=2,Name="instagram" },
                new ContactType(){Id=3,Name="facebook"}
            }
            );
        }
    }
}
