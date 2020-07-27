using System.Collections.Generic;
using System.IO;
using System.Linq;
using LuaToCs.Utils;

namespace LuaToCs
{
    public class Env
    {
        public static Env Instance { get; set; }

        private string _fileName;
        private List<Scope> _scopes;
        private HashSet<string> _dependencies;
        private Dictionary<string, string> _dependenciesDict;

        public FileGen fileGen;
        public TypeGen typeGen;
        public CodeGen codeGen;

        public string className;

        public string currentFuncSignature;
        public string currentFuncName;
        public bool isCurrentFuncCtor;
        public bool hasBeenCtor;
        public bool hasFuncReturnedValue = false;
        public readonly List<string> currentFuncArgs = new List<string>();
        public bool isLambda;

        public bool IsInFunc => currentFuncName != null;

        public void OpenScope()
        {
            _scopes.Add(Scope.FromScopes(_scopes.ToList()));
            codeGen.OpenScope();
        }

        public void CloseScope()
        {
            _scopes.RemoveAt(_scopes.Count - 1);
            codeGen.CloseScope();
        }

        public Env(string inputFileName, string fileName)
        {
            Init(inputFileName, fileName);
            Instance = this;
        }

        public void Init(string inputFileName, string fileName)
        {
            _fileName = fileName;
            className = NameResolver.ToPascalCase(Path.GetFileNameWithoutExtension(inputFileName));
            _scopes = new List<Scope>
            {
                new Scope()
            };

            _dependencies = new HashSet<string>();
            _dependenciesDict = new Dictionary<string, string>();

            fileGen = new FileGen(_fileName);

            typeGen = fileGen.Class();
            
            codeGen = typeGen.GetCodeGen();
        }

        public void EndClass()
        {
            typeGen.End(className);
        }

        public void Save()
        {
            fileGen.Save();
        }

        public Operand MemberFromString(ref string str, ref bool isSelf)
        {
            if (str == "self")
            {
                str = "this";
                isSelf = true;
            }

            return new Var(str);
        }

        public void AddField(string name)
        {
            typeGen.AddField(name);
        }

        public void AddFieldWithInitializer(Operand op1, Operand op2)
        {
            typeGen.AddFieldWithInitializer(op1.ToString(), op2);
        }

        public void SetClassName(string name)
        {
            if (name.Length > 1)
            {
                className = name;
            }
        }

        public void AddLocalField(string name)
        {
            _scopes.Last().Vars[name] = new Operand();
        }

        public bool ExistsVarInCurrentScope(string name)
        {
            if (name == "this") return true;
            return _scopes.Last().Vars.ContainsKey(name) || typeGen.HasField(name) || currentFuncArgs.Contains(name);
        }

        public void AddDependency(string dependency)
        {
            var name = CreateDependencyName(dependency);
            _dependencies.Add(name);
        }

        private static string CreateDependencyName(string dependency)
        {
            var strings = dependency.Split('.');
            var name = NameResolver.ToPascalCase(strings.Last());
            return name;
        }

        public void AddDependency(string localName, string dependency)
        {
            var name = CreateDependencyName(dependency);
            _dependenciesDict.Add(localName, name);
        }

        public bool HasDependency(string dependency)
        {
            return _dependencies.Contains(dependency);
        }

        public List<KeyValuePair<string, string>> GetDependencyInitializers()
        {
            return _dependenciesDict.ToList();
        }
        
        public Operand IntFromString(string str)
        {
            return long.Parse(str);
        }
        
        public Operand NumberFromString(string str)
        {
            return new NumberLiteral(str);
        }

        public Operand RealFromString(string str)
        {
            if (str.EndsWith("f"))
                return float.Parse(str.Substring(0, str.Length - 1), System.Globalization.CultureInfo.InvariantCulture);
            if (str.EndsWith("m"))
                return decimal.Parse(str.Substring(0, str.Length - 1),
                    System.Globalization.CultureInfo.InvariantCulture);
            return double.Parse(str, System.Globalization.CultureInfo.InvariantCulture);
        }

        public Operand StringFromString(string str)
        {
            return str.Substring(1, str.Length - 2);
        }

        public Operand BoolFromString(string str)
        {
            return bool.Parse(str);
        }

        public Operand OperandFromOperator(string oper, Operand lval, Operand rval)
        {
            if (lval == null || rval == null) return new Operand();
            switch (oper)
            {
                case "*":
                    return lval * rval;
                case "/":
                    return lval / rval;
                case "+":
                case "..":
                    return lval + rval;
                case "-":
                    return lval - rval;
                case "%":
                    return lval % rval;
                case ">":
                    return lval > rval;
                case "<":
                    return lval < rval;
                case ">=":
                    return lval >= rval;
                case "<=":
                    return lval <= rval;
                case "==":
                    return lval.EQ(rval);
                case "~=":
                    return lval.NE(rval);
                case "&":
                    return lval & rval;
                case "|":
                    return lval | rval;
                case "and":
                    return lval && rval;
                case "or":
                    return lval || rval;
                case "^":
                    return lval ^ rval;
                case "<<":
                    return lval.LeftShift(rval);
                case ">>":
                    return lval.RightShift(rval);
                case ".":
                case ":":
                    return lval.Dot(rval);


                default:
                    return rval;
            }
        }

        public Operand OperandFromOperator(string oper, Operand val)
        {
            switch (oper)
            {
                case "+":
                    return +val;
                case "-":
                    return -val;
                case "not":
                    return !val;
                case "#":
                    return val.ArrayLength();

                default:
                    return val;
            }
        }

        public Operand AssignOperandFromOperator(string oper, Operand lval, Operand rval, bool scoped)
        {
            return oper == "=" ? lval.Assign(rval, scoped) : new Operand();
        }
    }
}