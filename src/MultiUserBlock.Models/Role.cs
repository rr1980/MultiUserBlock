using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MultiUserBlock.Common.Enums;

namespace MultiUserBlock.Models
{
    public class Role
    {
        public int RoleId { get; set; }
        public UserRoleType UserRoleType { get; set; }
        public virtual ICollection<RoleToUser> RoleToUsers { get; set; }
    }
}