﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiUserBlock.Common.ViewModels
{
    public interface IHomeViewModel
    {
        IUserViewModel CurrentUser { get; set; }
    }
}