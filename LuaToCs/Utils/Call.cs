using System.Collections.Generic;

namespace LuaToCs.Utils
{
    public class Call : Operand
    {
        private readonly string _name;
        private readonly List<Operand> _args;

        public Call(string name, List<Operand> args)
        {
            NameResolver.Resolve(ref name);
            _name = name;
            _args = args;
        }

        public override string ToString()
        {
            if (Env.Instance.HasDependency(_name))
            {
                return $"new {_name}({string.Join(", ", _args)})";
            }
            return $"{_name}({string.Join(", ", _args)})";
        }
    }
}