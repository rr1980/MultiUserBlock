﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MultiUserBlock.Common;
using MultiUserBlock.Common.Enums;

namespace MultiUserBlock.ViewModels
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        public int Anrede { get; set; }
        [Required(ErrorMessage = "Name " + ErrorMessage.REQUIRED)]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Name " + ErrorMessage.MINMAXLENGTH)]
        public string Name { get; set; }
        public string Vorname { get; set; }
        [Required(ErrorMessage = "Username " + ErrorMessage.REQUIRED)]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Username " + ErrorMessage.MINMAXLENGTH)]
        public string Username { get; set; }
        public string ShowName { get; set; }
        public string Password { get; set; }
        public string Postleitzahl { get; set; }
        public string Stadt { get; set; }
        public string Strasse { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }

        public LayoutThemeViewModel LayoutThemeViewModel { get; set; }
        public IEnumerable<int> Roles { get; set; }
    }
}
