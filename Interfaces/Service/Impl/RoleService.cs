using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Domain.Enum;
using UserManagement.Domain.Model;
using UserManagement.Interfaces.Repository;

namespace UserManagement.Interfaces.Service.Impl
{
    public class RoleService : IRoleService
    {
        private IRoleRepository _repository;

        public RoleService(IRoleRepository repository)
        {
            _repository = repository;
        }

        public async Task<Role> FindByCodeAsync(string Code)
        {
            return await _repository.FindByCodeAsync(Code);
        }

        public bool IsRoleValid(RoleType roleType)
        {
            return roleType != RoleType.None;
        }
    }
}
