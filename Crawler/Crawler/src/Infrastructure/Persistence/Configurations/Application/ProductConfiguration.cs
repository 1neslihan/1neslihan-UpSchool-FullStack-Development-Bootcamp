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
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            //ID
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            //ProductId
            //builder.HasKey(x => x.OrderId);
            ////builder.Property(x => x.OrderId).ValueGeneratedOnAdd();

            //Name
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(150);
            

            //Picture
            builder.Property(x => x.Picture)
                .IsRequired()
                .HasMaxLength(500);

            //IsOnSale
            builder.Property(x => x.IsOnSale).IsRequired();

            //Price
            builder.Property(x => x.Price)
                .HasColumnType("decimal(18, 2)") // Fiyat alanının veritabanındaki sütun tipini belirtme
                .IsRequired();

            //SalePrice
            builder.Property(x => x.SalePrice)
                .HasColumnType("decimal(18, 2)");

            //Common Fields

            //CreatedOn
            builder.Property(x => x.CreatedOn)
                .IsRequired()
                .ValueGeneratedOnAdd();

            //CreatedByUserId
            builder.Property(x => x.CreatedByUserId)
                .IsRequired(false)
                .HasMaxLength(100);

            //ModifiedOn
            builder.Property(x => x.ModifiedOn).IsRequired(false);

            //ModifiedByUserId
            builder.Property(x => x.ModifiedByUserId)
                .IsRequired(false)
                .HasMaxLength(100);

            //DeletedOn
            builder.Property(x => x.DeletedOn).IsRequired(false);

            //DeletedByUserId
            builder.Property(x => x.DeletedByUserId)
                .IsRequired(false)
                .HasMaxLength(100);

            //IsDeleted
            builder.Property(x => x.IsDeleted).IsRequired();
            builder.Property(x => x.IsDeleted).HasDefaultValueSql("0");
            builder.HasIndex(x => x.IsDeleted);

            

            builder.ToTable("Products");
        }
    }
}
