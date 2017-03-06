using System.Collections.Generic;

namespace MultiUserBlock.Common.ViewModels
{
    public interface IUserViewModel
    {
        int Anrede { get; set; }
        string Email { get; set; }
        ILayoutThemeViewModel LayoutThemeViewModel { get; set; }
        string Name { get; set; }
        string Password { get; set; }
        string Postleitzahl { get; set; }
        IEnumerable<int> Roles { get; set; }
        string ShowName { get; set; }
        string Stadt { get; set; }
        string Strasse { get; set; }
        string Telefon { get; set; }
        int UserId { get; set; }
        string Username { get; set; }
        string Vorname { get; set; }
    }
}