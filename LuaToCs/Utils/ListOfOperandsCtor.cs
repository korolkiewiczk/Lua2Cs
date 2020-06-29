using System.Collections.Generic;
using System.Text;

namespace LuaToCs.Utils
{
    public class ListOfOperandsCtor : ListOfOperands
    {
        public ListOfOperandsCtor() : this(null)
        {
        }

        public ListOfOperandsCtor(List<Operand> operands, List<Operand> expressions = null, bool local = false) : base(
            operands, expressions, local)
        {
        }

        public override string ToString()
        {
            if (_operands == null)
            {
                return "new LuaObject()";
            }

            StringBuilder sb = new StringBuilder();
            string emptyExp = "new object()";
            sb.Append("new LuaObject(new Dictionary<object, object> {");
            List<string> expressions = new List<string>();
            for (int i = 0, j = 1; i < _operands.Count; i++)
            {
                if (_expressions?[i] == null)
                {
                    var assignment = _operands[i] as Assignment;
                    if (assignment != null)
                    {
                        string exp = assignment.Value.ToString();
                        exp = string.IsNullOrWhiteSpace(exp) ? emptyExp : exp;
                        expressions.Add($"[\"{assignment.Operand}\"] = {exp}");
                    }
                    else
                    {
                        string exp = _operands[i]?.ToString();
                        exp = string.IsNullOrWhiteSpace(exp) ? emptyExp : exp;
                        expressions.Add($"[{j++}] = {exp}");
                    }
                }
                else
                {
                    string exp = _expressions[i].ToString();
                    exp = string.IsNullOrWhiteSpace(exp) ? emptyExp : exp;
                    expressions.Add($"[{_operands[i]}] = {exp}");
                }
            }

            sb.Append(string.Join(",", expressions));
            sb.Append("})");

            return sb.ToString();
        }
    }
}