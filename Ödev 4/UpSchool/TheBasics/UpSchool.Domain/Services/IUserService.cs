using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UpSchool.Domain.Entities;

namespace UpSchool.Domain.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAllAsync(CancellationToken cancellationToken);
        Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Guid> AddAsync(string firstName, string lastName, int age, string email, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(User user, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
