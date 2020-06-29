namespace LuaToCs.Utils
{
    public class IntLiteral : Operand
    {
        private readonly int _value;

        public IntLiteral(int value)
        {
            _value = value;
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}