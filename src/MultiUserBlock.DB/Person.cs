﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MultiUserBlock.Common.Enums;

namespace MultiUserBlock.DB
{
    public class Person
    {
        public Anrede Anrede { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Vorname { get; set; }
        public string Postleitzahl { get; set; }
        public string Stadt { get; set; }
        public string Strasse { get; set; }
        public string Telefon { get; set; }
    }
}
