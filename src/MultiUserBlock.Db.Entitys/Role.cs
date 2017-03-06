using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MultiUserBlock.Common;
using MultiUserBlock.Common.Enums;

namespace MultiUserBlock.Db.Entitys
{
    public class Role : IEntitys
    {
        public UserRoleType UserRoleType { get; set; }
        public virtual ICollection<RoleToUser> RoleToUsers { get; set; }
    }
}