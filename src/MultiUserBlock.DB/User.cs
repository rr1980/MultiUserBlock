using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultiUserBlock.DB
{
    public class User : Person
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public virtual LayoutTheme LayoutTheme { get; set; }
        public virtual ICollection<RoleToUser> RoleToUsers { get; set; }
    }
}