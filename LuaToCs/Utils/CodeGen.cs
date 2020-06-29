using System;
using System.Collections.Generic;

namespace LuaToCs.Utils
{
    public class CodeGen
    {
        private readonly StringBuilderWithIdent _sb;
        private readonly int _startIdent;
        public bool IsScoped => _sb.Ident != _startIdent;

        public CodeGen(StringBuilderWithIdent sb)
        {
            _sb = sb;
            _startIdent = sb.Ident;
        }

        public void OpenScope()
        {
            _sb.AppendLine("{");
            _sb.Ident++;
        }

        public void CloseScope()
        {
            _sb.Ident--;
            _sb.AppendLine("}");
        }

        public void If(Operand op1)
        {
            _sb.AppendLine($"if ({op1})");
        }

        public void Else()
        {
            _sb.AppendLine("else");
        }

        public void End()
        {
            _sb.AppendLineNoIdent(";");
        }

        public void Switch(Operand op1)
        {
            _sb.AppendLine($"switch ({op1})");
        }

        public void While(Operand op1)
        {
            _sb.AppendLine($"while ({op1})");
        }

        public void WhileNeg(Operand op1)
        {
            _sb.AppendLine($"while (!({op1}))");
        }

        public void Do()
        {
            _sb.AppendLine("do");
        }

        public void ForEach(Operand op1, Operand op2)
        {
            _sb.AppendLine($"foreach (var ({op1.ToString().Replace(";", ",")}) in {op2})");
        }

        public void Break()
        {
            _sb.Append("break");
        }

        public void WriteLine(Operand[] op)
        {
            _sb.AppendLine($"Console.WriteLine({string.Join<Operand>(",", op)})");
        }

        public void For(Operand indexer, Operand startIndex, Operand max, Operand step)
        {
            string f3;
            string cmp = "<=";
            if (step == null)
            {
                f3 = indexer.ToString() + "++";
            }
            else
            {
                decimal stepd;
                Decimal.TryParse(step.ToString(), out stepd);
                if (stepd > 0)
                {
                    f3 = indexer.ToString() + "+=" + step.ToString();
                }
                else
                {
                    f3 = indexer.ToString() + "-=" + step.ToString();
                    cmp = ">=";
                }
            }

            _sb.AppendLine($"for (var {indexer} = {startIndex}; {indexer} {cmp} {max}; {f3})");
        }

        public void Return(Operand op1)
        {
            if (op1 == null) Return();
            else
            {
                Operand result = op1;
                var operands = op1 as ListOfOperands;
                if (operands != null)
                {
                    result = operands.ToCtor();
                }

                Env.Instance.hasFuncReturnedValue = true;
                _sb.Append($"return {result}");
            }
        }

        public void Return()
        {
            _sb.Append("return");
        }

        public void EndFunc()
        {
            if (!Env.Instance.hasFuncReturnedValue && !string.IsNullOrEmpty(Env.Instance.currentFuncSignature))
            {
                _sb.Replace(Env.Instance.currentFuncSignature, $"public void {Env.Instance.currentFuncName}");
            }

            if (Env.Instance.isCurrentFuncCtor)
            {
                _sb.AppendLine(Constants.Dependencies);
            }

            Env.Instance.currentFuncArgs.Clear();
            Env.Instance.hasFuncReturnedValue = false;
            Env.Instance.currentFuncName = null;
            Env.Instance.currentFuncSignature = null;
        }

        public void Emit(Operand operand)
        {
            _sb.Append(operand);
        }
    }
}