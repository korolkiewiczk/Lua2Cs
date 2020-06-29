namespace LuaToCs.Utils
{
    public class Var : Operand
    {
        private readonly string _name;

        public Var(string name)
        {
            NameResolver.Resolve(ref name);
            _name = name;
        }

        public override string ToString()
        {
            return _name;
        }
    }
}