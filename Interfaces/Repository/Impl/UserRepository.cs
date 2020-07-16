using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Data;
using UserManagement.Domain.Model;
using UserManagement.Util;

namespace UserManagement.Interfaces.Repository.Impl
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;

        public UserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task CreateAsync(User user)
        {
            await _dataContext.Users.AddAsync(user);
            await _dataContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _dataContext.Users.Update(user);
            await _dataContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            _dataContext.Users.Remove(user);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<User> FindByUsernameAsync(string username)
        {
            return await _dataContext.Users
                .Include(user => user.Role)
                .FirstOrDefaultAsync(user => user.Username == username);
        }

        public async Task<User> FindByUsernameAndSecretAsync(string username, string secret)
        {
            return await _dataContext.Users
                .Include(user => user.Role)
                .FirstOrDefaultAsync(user => 
                    user.Username == username && 
                    user.Secret == HashUtil.SHA1(secret)
            );
        }

        public async Task<IEnumerable<User>> FindByRoleAsync(Role role)
        {
            return await _dataContext.Users
                .Where(user => user.Role == role)
                .OrderByDescending(user => user.CreatedAt)
                .ToListAsync();
        }
    }
}
