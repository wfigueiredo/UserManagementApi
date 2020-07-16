using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Domain.Model
{
    [Table(name: "auditlog")]
    public class AuditLog : AbstractEntity<AuditLog, long>
    {
        [Column(name: "action")]
        public string Action { get; set; }

        [Column(name: "entity")]
        public string Entity { get; set; }

        [ForeignKey(name: "userid")]
        public User User { get; set; }

        [ForeignKey(name: "targetid")]
        public User Target { get; set; }

        [NotMapped]
        public new DateTime LastModified { get; set; }
    }
}
