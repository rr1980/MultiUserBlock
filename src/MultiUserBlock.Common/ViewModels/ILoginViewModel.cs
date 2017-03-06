
namespace MultiUserBlock.Common.ViewModels
{
    public interface ILoginViewModel
    {
        IUserViewModel CurrentUser { get; set; }
        string Password { get; set; }
        string ReturnUrl { get; set; }
        string Username { get; set; }
    }
}