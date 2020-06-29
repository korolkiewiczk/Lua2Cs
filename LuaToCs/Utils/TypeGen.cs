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
            _sb.AppendLine($"public class {Constants.ClassName} : LuaObject");
            _sb.AppendLine("{");
            _sb.Ident++;
            _sb.AppendLineNoIdent(Constants.Fields);
            _sb.AppendLineNoIdent(Constants.FieldsInit);

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
            _sb.AppendLine("}");
            _sb.Replace(Constants.ClassName, className);
            _sb.Replace(Constants.Fields, GetFieldsList());
            _fieldsInit.Remove(className);
            _sb.Replace(Constants.FieldsInit, GetFieldsListInit());
        }

        private string GetFieldsList()
        {
            StringBuilderWithIdent sb = new StringBuilderWithIdent {Ident = 1};
            foreach (var field in _fields)
            {
                sb.AppendLine($"private dynamic {field};");
            }

            return sb.ToString();
        }

        private string GetFieldsListInit()
        {
            StringBuilderWithIdent sb = new StringBuilderWithIdent {Ident = 1};
            foreach (var field in _fieldsInit)
            {
                sb.AppendLine($"private dynamic {field.Key} = {field.Value};");
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