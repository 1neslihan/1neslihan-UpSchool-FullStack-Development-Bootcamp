using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpSchool.Domain.Entities;
using UpSchool.Persistence.EntityFramework.Configurations;
using UpSchool.Persistence.EntityFramework.Seeders;

namespace UpSchool.Persistence.EntityFramework.Contexts
{
    public class UpStorageDbContext:DbContext
    {
        public DbSet<Account> Accounts { get; set; } //table added

        public UpStorageDbContext(DbContextOptions<UpStorageDbContext> options):base(options) 
        { 

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder) //db configurations added
        {
            //Configurations
            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            //seeders
            modelBuilder.ApplyConfiguration(new AccountSeeder());
            base.OnModelCreating(modelBuilder);
        }



    }
}
