namespace LuaToCs.Utils
{
    public class LongLiteral : Operand
    {
        private readonly long _value;

        public LongLiteral(long value)
        {
            _value = value;
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}