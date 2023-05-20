using Domain.Entities;
using Domain.Enums;
using Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Configurations.Application
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            
            
            builder.Property(x=>x.AddressType).IsRequired();    
            builder.Property(x => x.AddressType).HasConversion<short>();
            //builder.HasKey(x => new { x.AddressType });

            //RelationShips
            //One user can have many addresses.
            builder.HasOne<User>(x => x.User)
                .WithMany(x => x.Addresses)
                .HasForeignKey(x => x.UserId);

            #region Added by Neslihan
            builder.ToTable("Addresses");
            #endregion



        }
    }
}
