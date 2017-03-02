using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultiUserBlock.DB
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Vorname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public virtual ICollection<RoleToUser> RoleToUsers { get; set; }
    }
}