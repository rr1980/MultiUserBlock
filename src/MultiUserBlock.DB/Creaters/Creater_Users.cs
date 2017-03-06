using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MultiUserBlock.Common.Enums;
using MultiUserBlock.Db.Entitys;
using MultiUserBlock.DB;

namespace MultiUserBlock.Db.Creaters
{
    public static class Creater_Users
    {
        internal static void Create(DataContext context, UserRoleType[] roles, User user)
        {
            //user.Roles = context.Roles.Where(r => roles.Contains(r.UserRoleType)).ToList();

            foreach (var role in roles)
            {
                var rtu = new RoleToUser();

                var ro = context.Roles.First(r => r.UserRoleType == role);

                rtu.Role = ro;
                rtu.User = user;

                context.RoleToUsers.Add(rtu);
            }

            context.SaveChanges();
        }
    }
}
