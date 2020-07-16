using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Data;
using UserManagement.Domain.Model;

namespace UserManagement.Interfaces.Repository.Impl
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly DataContext _dataContext;

        public AuditLogRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task CreateAsync(AuditLog auditLog)
        {
            await _dataContext.AuditLogs.AddAsync(auditLog);
            await _dataContext.SaveChangesAsync();
        }
    }
}
