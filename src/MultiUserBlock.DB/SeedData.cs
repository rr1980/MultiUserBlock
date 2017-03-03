using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MultiUserBlock.Common.Enums;
using MultiUserBlock.Common.Repository;

namespace MultiUserBlock.DB
{
    public static class SeedData
    {
        public static void Seed(DataContext context)
        {
            if (context.Database.EnsureCreated())
            {
                Console.WriteLine("Database erzeugt...");
            }
            else
            {
                Console.WriteLine("Database existiert bereits...");
                return;
            }

            Console.WriteLine("Erzeuge Daten...");

            Creater_LayoutTheme.Create(context);

            Createer_Roles.Create(context);

            Creater_Users.Create(context,
                new UserRoleType[] { UserRoleType.Admin, UserRoleType.Default },
                new User
                {
                    Name = "Riesner",
                    Vorname = "Rene",
                    Username = "rr1980",
                    Password = "12003",
                    LayoutTheme = context.LayoutThemes.SingleOrDefault(lt => lt.Name == "default")
                });

            Creater_Users.Create(context,
                new UserRoleType[] { UserRoleType.Default },
                new User
                {
                    Name = "Riesner",
                    Vorname = "Sven",
                    Username = "Oxi",
                    Password = "12003",
                    LayoutTheme = context.LayoutThemes.SingleOrDefault(lt => lt.Name == "slate")
                });
        }
    }
}
