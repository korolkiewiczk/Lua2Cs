namespace LuaToCs.Utils
{
    public class FloatLiteral : Operand
    {
        private readonly float _value;

        public FloatLiteral(float value)
        {
            _value = value;
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}