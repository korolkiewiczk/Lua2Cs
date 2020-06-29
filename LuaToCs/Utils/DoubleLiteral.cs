namespace LuaToCs.Utils
{
    public class DoubleLiteral : Operand
    {
        private readonly double _value;

        public DoubleLiteral(double value)
        {
            _value = value;
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}