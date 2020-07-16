using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Domain.Model
{
    [Table(name: "role")]
    public class Role : AbstractEntity<Role, long>
    {
        [Column(name: "code")]
        public string Code { get; set; }

        [Column(name: "name")]
        public string Name { get; set; }

        [Column(name: "description")]
        public string Description { get; set; }
        
        [NotMapped]
        public new DateTime LastModified { get; set; }
    }
}
