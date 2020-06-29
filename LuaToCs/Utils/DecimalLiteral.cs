namespace LuaToCs.Utils
{
    public class DecimalLiteral : Operand
    {
        private readonly decimal _value;

        public DecimalLiteral(decimal value)
        {
            _value = value;
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}