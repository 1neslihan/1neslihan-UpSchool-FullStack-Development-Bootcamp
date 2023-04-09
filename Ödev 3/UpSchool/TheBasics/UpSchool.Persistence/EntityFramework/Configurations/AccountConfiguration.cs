using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpSchool.Domain.Entities;

namespace UpSchool.Persistence.EntityFramework.Configurations
{
    public class AccountConfiguration:IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            //Id
            builder.HasKey(x => x.Id);

            //Title
            builder.Property(x => x.Title).IsRequired();
            builder.Property(x => x.Title).HasMaxLength(150);
            //builder.HasIndex(x => x.Title);

            builder.HasIndex(x => new {x.Title, x.UserName});

            //UserName
            builder.Property(x => x.UserName).IsRequired();
            builder.Property(x => x.UserName).HasMaxLength(100);

            //Password
            builder.Property(x => x.Password).IsRequired();
            builder.Property(x => x.Password).HasMaxLength(100);

            //Url
            builder.Property(x => x.Url).IsRequired(false);
            builder.Property(x => x.Url).HasMaxLength(500);

            //IsFavorite
            builder.Property(x => x.IsFavorite).IsRequired();

            //CreatedOn
            builder.Property(x => x.CreatedOn).IsRequired();

            //LastModifiedOn
            builder.Property(x => x.LastModifiedOn).IsRequired(false);

            //declare table name
            builder.ToTable("Accounts");
        }

       
    }
}
