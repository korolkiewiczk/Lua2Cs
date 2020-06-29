namespace LuaToCs.Utils
{
    public class Assignment : Operand
    {
        private readonly Operand _operand;
        private readonly Operand _value;
        private readonly bool _local;
        private readonly bool _scoped;

        public Assignment(Operand operand, Operand value, bool local, bool scoped)
        {
            _operand = operand;
            _value = value;
            _local = local;
            _scoped = scoped;
        }

        public Operand Operand
        {
            get { return _operand; }
        }

        public Operand Value
        {
            get { return _value; }
        }

        public override string ToString()
        {
            if (!_scoped)
            {
                return $"public static dynamic {Operand} = {Value}";
            }

            var var = _local ? "var " : "";
            if (Env.Instance.ExistsVarInCurrentScope(Operand.ToString())) var = "";
            Env.Instance.AddLocalField(Operand.ToString());
            /*if (_operand is Var)
            {
                var variable = (_operand as Var).GetFirstPartOfName();
                if (!_local && !Env.Instance.ExistsVarInCurrentScope(variable) && variable == _operand.ToString())
                {
                    var = "var ";
                }
                if (!_local)
                    Env.Instance.AddField(variable);
                else
                    Env.Instance.AddLocalField(variable);
            }*/

            if (_local && Value.ToString().Contains("LuaObject()") && !Operand.ToString().Contains("this.")) var = "dynamic ";
            return $"{var}{Operand} = {Value}";
        }
    }
}