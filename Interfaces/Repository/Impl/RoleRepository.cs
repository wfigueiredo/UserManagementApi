using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Data;
using UserManagement.Domain.Model;

namespace UserManagement.Interfaces.Repository.Impl
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DataContext _dataContext;

        public RoleRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Role> FindByCodeAsync(string Code)
        {
            return await _dataContext.Roles
                .FirstOrDefaultAsync(role => role.Code == Code);
        }
    }
}
