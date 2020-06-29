using System;

namespace LuaToCs.Utils
{
    public class FuncDef : Operand
    {
        private readonly Operand _name;
        private readonly FuncArguments _args;
        private readonly bool _isCtor; 

        public FuncDef(Operand name, FuncArguments args)
        {
            if (name.ToString() == "__init")
            {
                name = new Var(Env.Instance.className);
                _isCtor = true;
            }

            _name = name;
            _args = args;

            Env.Instance.currentFuncSignature = $"public dynamic {_name}";
            Env.Instance.currentFuncName = _name.ToString();
            Env.Instance.hasFuncReturnedValue = false;
            Env.Instance.currentFuncArgs.AddRange(args.Names);
            Env.Instance.isCurrentFuncCtor = _isCtor;
        }

        public override string ToString()
        {
            if (_isCtor)
            {
                return $"public {_name}({_args})" + Environment.NewLine;
            }
            return $"public dynamic {_name}({_args})" + Environment.NewLine;
        }
    }
}