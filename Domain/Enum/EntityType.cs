using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Domain.Enum
{
    public enum EntityType
    {
        [Display(Name = "user")]
        User
    }
}
