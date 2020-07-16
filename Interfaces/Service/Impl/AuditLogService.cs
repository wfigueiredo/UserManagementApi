using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Domain.Model;
using UserManagement.Interfaces.Repository;

namespace UserManagement.Interfaces.Service.Impl
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IAuditLogRepository _repository;

        public AuditLogService(IAuditLogRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateAsync(AuditLog auditLog)
        {
            await _repository.CreateAsync(auditLog);
        }
    }
}
