using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MultiUserBlock.Common.Enums;

namespace MultiUserBlock.DB
{
    public static class Creater_Mieter
    {
        internal static void Create(DataContext context, Mieter mieter)
        {
            context.Mieters.Add(mieter);

            context.SaveChanges();
        }
    }
}
