using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Domain.Model;

namespace UserManagement.Interfaces.Repository
{
    public interface IUserRepository
    {
        Task CreateAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task<User> FindByUsernameAsync(string username);
        Task<User> FindByUsernameAndSecretAsync(string username, string secret);
        Task<IEnumerable<User>> FindByRoleAsync(Role role);
    }
}
