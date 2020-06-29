namespace LuaToCs.Utils
{
    public class StringLiteral : Operand
    {
        private readonly string _value;

        public StringLiteral(string value)
        {
            _value = value.Replace("\"","\\\"");
        }

        public override string ToString()
        {
            return $"\"{_value}\"";
        }
    }
}