using System;
using System.Collections.Generic;
using System.IO;
using LuaToCs;

namespace l2cs
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            ProjectConverter projectConverter = new ProjectConverter(args[0], args.Length > 1 ? args[1] : "out");
            projectConverter.Convert();
        }
    }
}