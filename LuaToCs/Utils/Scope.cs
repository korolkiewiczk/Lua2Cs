using System.Collections.Generic;

namespace LuaToCs.Utils
{
    internal class Scope
    {
        private Dictionary<string, Operand> _vars;

        public Dictionary<string, Operand> Vars
        {
            get
            {
                if (_vars == null)
                {
                    _vars = new Dictionary<string, Operand>();
                }

                return _vars;
            }
        }

        public Scope()
        {
        }

        public static Scope FromScopes(List<Scope> scopes)
        {
            Scope scope = new Scope();
            foreach (var iscope in scopes)
            {
                foreach (var var in iscope.Vars)
                {
                    //if (!scope.Vars.ContainsKey(var.Key))
                    //scope.Vars.Add(var.Key, var.Value);
                    scope.Vars[var.Key] = var.Value;
                }
            }

            return scope;
        }

        public void AddVar(string name, Operand op)
        {
            Vars[name] = op;
        }
    }
}