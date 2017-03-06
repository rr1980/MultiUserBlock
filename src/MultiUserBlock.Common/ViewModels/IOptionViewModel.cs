using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiUserBlock.Common.ViewModels
{
    public interface IOptionViewModel
    {
        IUserViewModel CurrentUser { get; set; }
        List<ILayoutThemeViewModel> LayoutThemeViewModels { get; set; }
    }
}
