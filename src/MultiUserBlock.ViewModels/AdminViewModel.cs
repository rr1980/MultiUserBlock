using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiUserBlock.ViewModels
{
    public class AdminViewModel
    {
        public IEnumerable<UserViewModel> Users { get; set; }
        public List<string> Errors { get; set; }
    }
}
