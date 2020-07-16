using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Data.Mappings;
using UserManagement.Domain.Model;

namespace UserManagement.Data
{
    public class DataContext : DbContext
    {
        private const string DEFAULT_SCHEMA = "usermanagementapi";

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RoleMapping());
            modelBuilder.ApplyConfiguration(new UserMapping());
            modelBuilder.ApplyConfiguration(new AuditLogMapping());

            modelBuilder.HasDefaultSchema(DEFAULT_SCHEMA);
        }
    }
}
