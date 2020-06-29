namespace LuaToCs.Utils
{
    public class Conditional : Operand
    {
        private readonly Operand _operand;
        private readonly Operand _ifTrue;
        private readonly Operand _ifFalse;

        public Conditional(Operand operand, Operand ifTrue, Operand ifFalse)
        {
            _operand = operand;
            _ifTrue = ifTrue;
            _ifFalse = ifFalse;
        }

        public override string ToString()
        {
            return $"{_operand}? {_ifTrue} : {_ifFalse}";
        }
    }
}