using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MultiUserBlock.Common.Enums;
using MultiUserBlock.Common.Repository;

namespace MultiUserBlock.DB
{
    public static class SeedData
    {
        public static void Seed(DataContext context,bool del,bool create)
        {
            if (del)
            {
                _del(context);
            }

            if (create)
            {
                if (_create(context))
                {
                    _build(context);
                }
            }
        }

        public static void Seed(DataContext context)
        {
            if (_create(context))
            {
                _build(context);
            }
        }

        internal static void _build(DataContext context)
        {
            Console.WriteLine("Erzeuge Daten...");

            Creater_LayoutTheme.Create(context);

            Createer_Roles.Create(context);

            Creater_Mieter.Create(context);

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

        private static void _del(DataContext context)
        {
            Console.WriteLine("Versuche Database zu löschen, einen Moment bitte...");
            if (context.Database.EnsureDeleted())
            {
                Console.WriteLine("Database gelöscht...");
            }
            else
            {
                Console.WriteLine("Keine Database gefunden...");
            }
        }

        private static bool _create(DataContext context)
        {
            Console.WriteLine("Versuche Database zu erzeugen, einen Moment bitte...");

            if (context.Database.EnsureCreated())
            {
                Console.WriteLine("Database erzeugt...");
                return true;
            }
            else
            {
                Console.WriteLine("Database existiert bereits...");
                return false;
            }
        }
    }
}
