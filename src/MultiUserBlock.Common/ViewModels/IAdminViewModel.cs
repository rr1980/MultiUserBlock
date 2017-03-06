using System.Collections.Generic;

namespace MultiUserBlock.Common.ViewModels
{
    public interface IAdminViewModel
    {
        IUserViewModel CurrentUser { get; set; }
        List<string> Errors { get; set; }
        IEnumerable<IUserViewModel> Users { get; set; }
    }
}