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
        DbSet<Product> Products { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<OrderEvent> OrderEvents { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        int SaveChanges();
    }
}
