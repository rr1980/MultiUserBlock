using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiUserBlock.Common.ViewModels
{
    public interface ILayoutThemeViewModel
    {
        int Id { get; set; }
        string Name { get; set; }
        string Link { get; set; }
    }
}
