using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MultiUserBlock.Common;

namespace MultiUserBlock.Db.Entitys
{
    public class LayoutTheme : IEntitys
    {
        public string Name { get; set; }
        public string Link { get; set; }
    }
}
