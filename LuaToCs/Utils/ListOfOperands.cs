using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LuaToCs.Utils
{
    public class ListOfOperands : Operand
    {
        protected readonly List<Operand> _operands;
        protected readonly List<Operand> _expressions;
        private readonly bool _local;

        public ListOfOperands(List<Operand> operands, List<Operand> expressions = null, bool local = false)
        {
            if (expressions != null && operands != null && operands.Count < expressions.Count)
                throw new ArgumentException("Invalid number of operands or expressions");
            _operands = operands;
            _expressions = expressions;
            _local = local;
        }

        public static Operand ToOperand(Operand op1, Operand op2, bool local = false)
        {
            if (op1.GetType() != typeof(ListOfOperands) || op2.GetType() != typeof(ListOfOperands))
            {
                return new Assignment(op1, op2, local, true);
            }

            var operands = ((ListOfOperands) op1)._operands;
            var expressions = ((ListOfOperands) op2)._operands;
            return new ListOfOperands(operands, expressions, local);
        }

        public static Operand ToOperand(List<Operand> op1)
        {
            return op1.Count == 1 ? op1.First() : new ListOfOperands(op1);
        }

        public Operand ToCtor()
        {
            if (_operands.Count > 1)
            {
                return new ListOfOperandsCtor(_operands, _expressions, _local);
            }

            return this;
        }

        public override string ToString()
        {
            if (_expressions == null)
            {
                return string.Join(";", _operands.Select(x => _local ? $"dynamic {x}" : x));
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _expressions.Count; i++)
            {
                if (_local) sb.Append("var ");
                sb.Append($"{_operands[i]} = {_expressions[i]}; ");
            }

            return sb.ToString();
        }
    }
}