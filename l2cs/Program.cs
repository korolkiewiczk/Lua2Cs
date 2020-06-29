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
            /*var dir = "C:\\Projects\\external\\DeepHoldem\\Source\\";
            FileConverter c = new FileConverter(dir + "Tree\\tree_visualiser.lua", "out/out.txt");
            c.Convert();
            Console.WriteLine(c.CompilationErrorMessage);*/

            ProjectConverter projectConverter = new ProjectConverter(args[0], "out");
            projectConverter.Convert();
        }
    }
}