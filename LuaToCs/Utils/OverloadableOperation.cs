namespace LuaToCs.Utils
{
    public class OverloadableOperation : Operand
    {
        private readonly string _op;
        protected readonly Operand _left;
        protected readonly Operand _right;

        public OverloadableOperation(string op, Operand left, Operand right = null)
        {
            _op = op;
            _left = left;
            _right = right;
        }

        public override string ToString()
        {
            if (_right == null)
            {
                return $"{_op}{_left}";
            }

            return $"({_left} {_op} {_right})";
        }
    }
}