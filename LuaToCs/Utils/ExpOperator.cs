using System;

namespace LuaToCs.Utils
{
    public class ExpOperator : OverloadableOperation
    {
        public ExpOperator(Operand left, Operand right = null) : base(null, left, right)
        {
        }

        public override string ToString()
        {
            return $"Math.Pow({_left}, {_right})";
        }
    }
}