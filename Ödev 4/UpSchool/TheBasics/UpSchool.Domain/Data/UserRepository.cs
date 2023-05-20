using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UpSchool.Domain.Entities;

namespace UpSchool.Domain.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly SQLiteAsyncConnection _database;
        public UserRepository()
        {
            var dbPath = Path.Combine("C:\\Users\\DL_ne\\OneDrive\\Masaüstü", "upschool.db");
            _database= new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<User>().Wait();
        }

        public Task<int> AddAsync(User user, CancellationToken cancellationToken)
        {
            return _database.InsertAsync(user);
        }

        public Task<int> DeleteAsync(Expression<Func<User,bool>> predicate, CancellationToken cancellationToken)
        {
            return _database.Table<User>().DeleteAsync(predicate);
        }

        public Task<List<User>> GetAllAsync(CancellationToken cancellationToken)
        {
            return _database.Table<User>().ToListAsync();
        }

        public Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _database.Table<User>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<int> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            return _database.UpdateAsync(user);
        }
    }
}
