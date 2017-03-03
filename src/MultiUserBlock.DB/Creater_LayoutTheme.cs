using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiUserBlock.DB
{
    public static class Creater_LayoutTheme
    {
        private static DataContext _context;

        internal static void Create(DataContext context)
        {
            _context = context;
            _build("default", "//netdna.bootstrapcdn.com/bootstrap/3.0.0/css/bootstrap.min.css");
            _build("slate", "//bootswatch.com/slate/bootstrap.min.css");

            context.SaveChanges();
        }

        private static void _build(string name,string link)
        {
            LayoutTheme lt;
            lt = new LayoutTheme();
            lt.Name = name;
            lt.Link = link;

            _context.LayoutThemes.Add(lt);
        }
    }
}
