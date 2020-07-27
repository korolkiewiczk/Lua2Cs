using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace LuaToCs.Utils
{
    public static class NameResolver
    {
        public static void Resolve(ref string name)
        {
            var strings = name.Split('.');
            var newStrings = new List<string>();
            for (int i = 0; i < strings.Length; i++)
            {
                var str = strings[i];
                switch (str)
                {
                    case "params":
                        newStrings.Add("@params");
                        break;
                    case "long":
                        newStrings.Add("@long");
                        break;
                    case "byte":
                        newStrings.Add("@byte");
                        break;
                    case "float":
                        newStrings.Add("@float");
                        break;
                    case "out":
                        newStrings.Add("@out");
                        break;
                    case "string":
                        newStrings.Add("LuaString");
                        break;
                    case "base":
                        newStrings.Add("@base");
                        break;
                    default:
                        newStrings.Add(str);
                        break;
                }
            }

            if (newStrings[0] == "math")
            {
                for (int i = 0; i < newStrings.Count; i++)
                {
                    newStrings[i] = ToPascalCase(newStrings[i]);
                }
            }

            name = string.Join(".", newStrings);
        }

        public static void Resolve(List<string> names)
        {
            for (int i = 0; i < names.Count; i++)
            {
                var name = names[i];
                Resolve(ref name);
                names[i] = name;
            }
        }

        public static string ToPascalCase(this string str)
        {
            System.Text.StringBuilder resultBuilder = new System.Text.StringBuilder();

            foreach (char c in str)
            {
                // Replace anything, but letters and digits, with space
                if (!char.IsLetterOrDigit(c))
                {
                    resultBuilder.Append(" ");
                }
                else
                {
                    resultBuilder.Append(c);
                }
            }

            string result = resultBuilder.ToString();

            result = result.ToLower();

            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;

            result = myTI.ToTitleCase(result).Replace(" ", String.Empty);

            return result;
        }
        
        public static string ToCamelCase(this string str)
        {
            System.Text.StringBuilder resultBuilder = new System.Text.StringBuilder();

            foreach (char c in str)
            {
                // Replace anything, but letters and digits, with space
                if (!char.IsLetterOrDigit(c))
                {
                    resultBuilder.Append(" ");
                }
                else
                {
                    resultBuilder.Append(c);
                }
            }

            string result = resultBuilder.ToString();

            result = result.ToLower();

            var strings = result.Split(' ');
            if (strings.Length == 1) return strings[0];

            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;

            for (int i = 1; i < strings.Length; i++)
            {
                strings[i] = myTI.ToTitleCase(strings[i]);
            }

            return string.Join("", strings);
        }

        public static string ToPascalCase(this Operand op)
        {
            if (op == null) return null;
            return ToPascalCase(op.ToString());
        }
        
        public static string ToCamelCase(this Operand op)
        {
            if (op == null) return null;
            return ToCamelCase(op.ToString());
        }
    }
}