using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Domain.Enum
{
    public enum ActionType
    {
        [Display(Name = "create")]
        Create,

        [Display(Name = "update")]
        Update,

        [Display(Name = "delete")]
        Delete,
    }
}
