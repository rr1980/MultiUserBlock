using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MultiUserBlock.DB
{
    public class RoleToUser
    {
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
