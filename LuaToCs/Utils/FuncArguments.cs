using System.Collections.Generic;
using System.Linq;

namespace LuaToCs.Utils
{
    public class FuncArguments : Operand
    {
        private readonly List<string> _names;

        public FuncArguments()
        {
            _names = new List<string>();
        }

        public FuncArguments(List<string> names)
        {
            NameResolver.Resolve(names);
            _names = names;
        }

        public List<string> Names
        {
            get { return _names; }
        }

        public override string ToString()
        {
            return string.Join(", ", Names.Select(x => $"dynamic {x} = null"));
        }
    }
}