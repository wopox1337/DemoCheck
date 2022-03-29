﻿using System;
using System.Collections.Generic;

namespace DemoParser.Demo_stuff.GoldSource.Verify
{
    public class Category
    {
        public List<Tuple<string, Commandtype>> CommandRules;
        public List<Tuple<string, string>> CvarRules;
        public string name;

        public Category()
        {
            CommandRules = new List<Tuple<string, Commandtype>>();
            CvarRules = new List<Tuple<string, string>>();
        }
    }
}