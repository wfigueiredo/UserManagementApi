using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Domain.Enum;
using UserManagement.Domain.Model;

namespace UserManagement.Interfaces.Service
{
    public interface IRoleService
    {
        public Task<Role> FindByCodeAsync(string Code);
        public bool IsRoleValid(RoleType roleType);
    }
}
