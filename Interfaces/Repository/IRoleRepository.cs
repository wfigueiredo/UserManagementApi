using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Domain.Model;

namespace UserManagement.Interfaces.Repository
{
    public interface IRoleRepository
    {
        Task<Role> FindByCodeAsync(string Code);
    }
}
