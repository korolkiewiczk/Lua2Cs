using System.Collections.Generic;

namespace LuaToCs.Utils
{
    public class ArrayAccess : Operand
    {
        private readonly Operand _operand;
        private readonly List<Operand> _indexes;

        public ArrayAccess(Operand operand, List<Operand> indexes)
        {
            _operand = operand;
            _indexes = indexes;
        }

        public override string ToString()
        {
            return $"{_operand}[{string.Join("][", _indexes)}]";
        }
    }
}