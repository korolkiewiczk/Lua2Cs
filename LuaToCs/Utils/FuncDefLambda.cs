using System;
using System.Linq;

namespace LuaToCs.Utils
{
    public class FuncDefLambda : Operand
    {
        private readonly FuncArguments _args; 

        public FuncDefLambda(FuncArguments args)
        {
            _args = args;

            Env.Instance.isLambda = true;
            /*Env.Instance.currentFuncSignature = "";
            Env.Instance.currentFuncName = "";
            Env.Instance.hasFuncReturnedValue = false;
            Env.Instance.currentFuncArgs.AddRange(args.Names);
            Env.Instance.isCurrentFuncCtor = false;*/
        }

        public override string ToString()
        {
            return $"({string.Join(", ", _args.Names.Select(x => $"{x}"))}) =>";
        }
    }
}