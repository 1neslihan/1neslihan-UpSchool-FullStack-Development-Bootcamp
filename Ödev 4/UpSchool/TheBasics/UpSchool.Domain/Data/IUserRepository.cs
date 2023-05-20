using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UpSchool.Domain.Entities;

namespace UpSchool.Domain.Data
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync(CancellationToken cancellationToken);
        Task<User> GetByIdAsync(Guid id,CancellationToken cancellationToken);
        Task<int> AddAsync(User user,CancellationToken cancellationToken);
        Task<int> UpdateAsync(User user,CancellationToken cancellationToken);
        Task<int> DeleteAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken);
    }
}
