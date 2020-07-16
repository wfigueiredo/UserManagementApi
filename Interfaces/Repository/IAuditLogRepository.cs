using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Domain.Model;

namespace UserManagement.Interfaces.Repository
{
    public interface IAuditLogRepository
    {
        public Task CreateAsync(AuditLog auditLog);
    }
}
