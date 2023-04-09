using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Configurations.Application
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            //Relationships
            builder.HasMany<City>(x => x.Cities)
                .WithOne(x => x.Country)
                .HasForeignKey(x => x.CountryId);

            builder.ToTable("Countries");

           
        }
    }
}
