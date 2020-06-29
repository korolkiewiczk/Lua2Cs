namespace LuaToCs.Utils
{
    public class Invocation : Operand
    {
        private readonly string _name;
        private readonly Operand _operand;
        private readonly Operand[] _args;

        public Invocation(string name, Operand operand, Operand[] args)
        {
            _name = name;
            _operand = operand;
            _args = args;
        }

        public override string ToString()
        {
            return $"{_operand}.{_name}({string.Join<Operand>(",", _args)})";
        }
    }
}