using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Account> Accounts { get; set; }
        DbSet<Country> Countries { get; set; }
        DbSet<City> Cities { get; set; }
        
        #region Added by Neslihan
        DbSet<Address> Addresses { get; set; }
        //DbSet<Note> Notes { get; set; }
        DbSet<NoteCategory> NoteCategories { get; set; }
        #endregion
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        int SaveChanges();

    }
}
