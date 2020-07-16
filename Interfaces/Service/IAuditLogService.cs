using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Domain.Model;

namespace UserManagement.Interfaces.Service
{
    public interface IAuditLogService
    {
        Task CreateAsync(AuditLog auditLog);
    }
}
