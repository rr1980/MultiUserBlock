using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MultiUserBlock.Common;

namespace MultiUserBlock.ViewModels
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "Name " + ErrorMessage.REQUIRED)]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Name " + ErrorMessage.MINMAXLENGTH)]
        public string Name { get; set; }
        public string Vorname { get; set; }
        [Required(ErrorMessage = "Username " + ErrorMessage.REQUIRED)]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Username " + ErrorMessage.MINMAXLENGTH)]
        public string Username { get; set; }

        public string ShowName { get; set; }

        public string Password { get; set; }

        public LayoutThemeViewModel LayoutThemeViewModel { get; set; }
        public IEnumerable<int> Roles { get; set; }
    }
}
