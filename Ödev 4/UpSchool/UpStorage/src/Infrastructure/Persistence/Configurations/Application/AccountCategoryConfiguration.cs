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
    public class AccountCategoryConfiguration : IEntityTypeConfiguration<AccountCategory>
    {
        public void Configure(EntityTypeBuilder<AccountCategory> builder)
        {
            //ID
            builder.HasKey(x => new { x.AccountId, x.CategoryId });

            //Relationships
            builder.HasOne<Account>(x => x.Account)
                .WithMany(x => x.AccountCategories)
                .HasForeignKey(x => x.AccountId);

            builder.HasOne<Category>(x => x.Category)
                .WithMany(x => x.AccountCategories)
                .HasForeignKey(x => x.CategoryId);
        }
    }
}
