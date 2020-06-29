using System;

namespace LuaToCs.Utils
{
    public class EnumLiteral : Operand
    {
        private readonly Enum _value;

        public EnumLiteral(Enum value)
        {
            _value = value;
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}