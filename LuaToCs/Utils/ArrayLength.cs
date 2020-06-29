namespace LuaToCs.Utils
{
    public class ArrayLength : Operand
    {
        private readonly Operand _operand;

        public ArrayLength(Operand operand)
        {
            _operand = operand;
        }

        public override string ToString()
        {
            return $"{_operand}.Count()";
        }
    }
}