using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Domain.Model
{
    [Table(name: "user")]
    public class User : AbstractEntity<User, long>
    {
        [Column(name: "name")]
        public string Name { get; set; }

        [Column(name: "lastname")]
        public string LastName { get; set; }

        [Column(name: "username")]
        public string Username { get; set; }

        [Column(name: "secret")]
        public string Secret { get; set; }

        [Column(name: "emailaddress")]
        public string EmailAddress { get; set; }

        [Column(name: "phonenumber")]
        public string PhoneNumber { get; set; }

        [ForeignKey(name: "roleid")]
        public Role Role { get; set; }

        [Column(name: "active")]
        public bool Active { get; set; }
    }
}
