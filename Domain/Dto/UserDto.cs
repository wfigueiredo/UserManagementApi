using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Domain.Enum;
using UserManagement.Domain.Model;

namespace UserManagement.Domain.Dto
{
    public class UserDto
    {
        public string name { get; set; }
        public string lastname { get; set; }
        public string fullname { get; set; }

        public string username { get; set; }
        public string secret { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public RoleType role { get; set; }
        public string token { get; set; }
        
        [JsonIgnore]
        public bool active { get; set; }
    }
}
