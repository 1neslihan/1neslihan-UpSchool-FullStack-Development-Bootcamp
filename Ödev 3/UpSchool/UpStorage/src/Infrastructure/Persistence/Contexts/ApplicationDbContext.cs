using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Contexts
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }

        #region Added by Neslihan
        public DbSet<Address> Addresses { get; set; }

        //public DbSet<Note> Notes { get; set; }
        public DbSet<NoteCategory> NoteCategories { get; set; }

        #endregion

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
            

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //configurations
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            
            //ignores
            //modelBuilder.Ignore<User>();
            //modelBuilder.Ignore<Role>();
            //modelBuilder.Ignore<UserRole>();
            //modelBuilder.Ignore<UserToken>();
            //modelBuilder.Ignore<UserClaim>();
            //modelBuilder.Ignore<RoleClaim>();
            //modelBuilder.Ignore<UserLogin>();

            base.OnModelCreating(modelBuilder);
        }

    }
}
