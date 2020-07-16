using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Domain.Enum;
using UserManagement.Domain.Model;
using UserManagement.Extensions;
using UserManagement.Interfaces.Repository;

namespace UserManagement.Interfaces.Services.Impl
{
    public class UserService : IUserService
    {
        private static readonly IDictionary<string, string[]> RolePermissionEngine = new Dictionary<string, string[]>()
        {
            { RoleType.Root.GetDisplayName(), new string[]{ RoleType.Admin.GetDisplayName(), RoleType.User.GetDisplayName() }},
            { RoleType.Admin.GetDisplayName(), new string[]{ RoleType.User.GetDisplayName() }},
            { RoleType.User.GetDisplayName(), new string[]{ RoleType.User.GetDisplayName() }},
            { RoleType.Guest.GetDisplayName(), new string[]{ RoleType.Guest.GetDisplayName() }},
        };

        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
            
        }
        public async Task CreateAsync(User user)
        {
            user.Active = true;
            await _repository.CreateAsync(user);
        }

        public async Task DeleteAsync(User user)
        {
            user.Active = false;
            await _repository.UpdateAsync(user);
        }

        public async Task UpdateAsync(User user)
        {
            await _repository.UpdateAsync(user);

        }
        public async Task<User> FindByUsernameAsync(string username)
        {
            return await _repository.FindByUsernameAsync(username);
        }

        public async Task<User> FindByUsernameAndSecretAsync(string username, string secret)
        {
            return await _repository.FindByUsernameAndSecretAsync(username, secret);
        }

        public async Task<IEnumerable<User>> FindByRoleAsync(Role role)
        {
            return await _repository.FindByRoleAsync(role);
        }

        public bool HasActPermission(Role CurrentRole, Role RequestedRole)
        {
            string[] AllowedRoles;
            RolePermissionEngine.TryGetValue(CurrentRole.Code, out AllowedRoles);
            return Array.Exists(AllowedRoles, role => role == RequestedRole.Code);
        }

        
    }
}
