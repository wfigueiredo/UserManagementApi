using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Domain.Model
{
    public abstract class AbstractEntity<T, K> where T : class
    {
        [Column(name: "id")]
        public virtual K Id { get; set; }

        [Column(name: "createdat")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column(name: "lastmodified")]
        public DateTime LastModified { get; set; }
    }
}
