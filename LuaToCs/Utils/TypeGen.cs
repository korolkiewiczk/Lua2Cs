using System.Collections.Generic;

namespace LuaToCs.Utils
{
    public class TypeGen
    {
        private readonly StringBuilderWithIdent _sb;
        private readonly HashSet<string> _fields;
        private readonly Dictionary<string, Operand> _fieldsInit;

        public TypeGen(StringBuilderWithIdent sb)
        {
            _sb = sb;
            _sb.AppendLineA($"public class {Constants.ClassName} : LuaObject");
            _sb.AppendLineA("{");
            _sb.Ident++;
            _sb.AppendLineNoIdentA(Constants.Fields);
            _sb.AppendLineNoIdentA(Constants.FieldsInit);
            _sb.AppendLineNoIdentA(Constants.ImplicitCtor);

            _fields = new HashSet<string>();
            _fieldsInit = new Dictionary<string, Operand>();
        }

        public CodeGen GetCodeGen()
        {
            return new CodeGen(_sb);
        }

        public void End(string className)
        {
            _sb.Ident--;
            _sb.AppendLineA("}");
            _sb.Replace(Constants.ClassName, className);
            _sb.Replace(Constants.Fields, GetFieldsList());
            _fieldsInit.Remove(className);
            if (!Env.Instance.hasBeenCtor)
            {
                _sb.Replace(Constants.ImplicitCtor, 
$@"    public {className}()
    {{
    {Constants.Dependencies}
    {Constants.InitCode}
    }}");
            }
            else
            {
                _sb.Replace(Constants.ImplicitCtor, string.Empty);
            }
            _sb.Replace(Constants.FieldsInit, GetFieldsListInit());
            _sb.Replace(Constants.Dependencies, GetDependencyInit());
            _sb.ReplaceB(Constants.InitCode);
        }

        private string GetDependencyInit()
        {
            var sb = new StringBuilderWithIdent() {Ident = 2};
            foreach (var dependencyInitializer in Env.Instance.GetDependencyInitializers())
            {
                sb.AppendLineA($"this.{dependencyInitializer.Key} = new {dependencyInitializer.Value}();");
            }

            return sb.ToString();
        }

        private string GetFieldsList()
        {
            StringBuilderWithIdent sb = new StringBuilderWithIdent {Ident = 1};
            foreach (var field in _fields)
            {
                sb.AppendLineA($"public dynamic {field};");
            }

            return sb.ToString();
        }

        private string GetFieldsListInit()
        {
            StringBuilderWithIdent sb = new StringBuilderWithIdent {Ident = 1};
            foreach (var field in _fieldsInit)
            {
                sb.AppendLineA($"public dynamic {field.Key} = {field.Value};");
            }

            return sb.ToString();
        }

        public void AddField(string name)
        {
            _fields.Add(name);
        }

        public void AddFieldWithInitializer(string name, Operand init)
        {
            _fieldsInit[name] = init;
        }

        public bool HasField(string name)
        {
            return _fields.Contains(name);
        }
    }
}