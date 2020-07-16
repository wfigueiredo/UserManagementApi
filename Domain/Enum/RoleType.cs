using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Domain.Enum
{
    public enum RoleType
    {
        [Display(Name = null)]
        None,

        [Display(Name = "root")]
        Root,

        [Display(Name = "admin")]
        Admin,

        [Display(Name = "user")]
        User,

        [Display(Name = "guest")]
        Guest
    }
}
