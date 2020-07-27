namespace LuaToCs.Utils
{
    public class NumberLiteral : Operand
    {
        private readonly string _val;

        public NumberLiteral(string val)
        {
            _val = val;
        }

        public override string ToString()
        {
            return _val;
        }
    }
}