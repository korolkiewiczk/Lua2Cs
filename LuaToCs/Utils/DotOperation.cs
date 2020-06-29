namespace LuaToCs.Utils
{
    public class DotOperation : Operand
    {
        private readonly Operand _left;
        private readonly Operand _right;

        public DotOperation(Operand left, Operand right)
        {
            _left = left;
            _right = right;
        }

        public override string ToString()
        {
            return $"{_left}.{_right}";
        }
    }
}