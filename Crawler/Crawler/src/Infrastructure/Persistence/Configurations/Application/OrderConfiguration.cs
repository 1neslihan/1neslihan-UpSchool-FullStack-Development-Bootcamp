using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Configurations.Application
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            //Id
            builder.HasKey(x => x.Id);
            //builder.Property(x => x.Id).ValueGeneratedOnAdd();

            //Requested Amount
            builder.Property(x => x.RequestedAmount)
                .IsRequired(false);

            //Total Found Amount
            builder.Property(x=>x.TotalFoundAmount)
                .IsRequired(false);

            //Product Crawl Type
            builder.Property(x => x.ProductCrawlType)
                .IsRequired()
                .HasConversion<int>();

            //CreatedOn
            builder.Property(x => x.CreatedOn)
                .IsRequired()
                .ValueGeneratedOnAdd();

            //ModifiedOn
            builder.Property(x => x.ModifiedOn).IsRequired(false);

            //DeletedOn
            builder.Property(x => x.DeletedOn).IsRequired(false);

            //IsDeleted
            builder.Property(x => x.IsDeleted).IsRequired();
            builder.Property(x => x.IsDeleted).HasDefaultValueSql("0");
            builder.HasIndex(x => x.IsDeleted);

            //Relationships
            builder.HasMany<Product>(x => x.Products)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade); ;

            builder.HasMany<OrderEvent>(x=>x.OrderEvents)
                .WithOne(x=>x.Order)
                .HasForeignKey(x => x.OrderId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade); ;

            builder.ToTable("Orders");
        }
    }
}
