﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiUserBlock.ViewModels
{
    public class OptionViewModel
    {
        public UserViewModel CurrentUser { get; set; }
        public List<LayoutThemeViewModel> LayoutThemeViewModels { get; set; }
    }
}