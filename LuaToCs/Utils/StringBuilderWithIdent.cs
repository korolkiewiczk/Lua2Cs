using System.Text;

namespace LuaToCs.Utils
{
    public class StringBuilderWithIdent
    {
        private StringBuilder _sb = new StringBuilder();
        private StringBuilder _sbNoScoped = new StringBuilder();
        public int Ident { get; set; }
        private bool IsScoped => Env.Instance.IsInFunc;
        private StringBuilder Sb => IsScoped ? _sb : _sbNoScoped;

        private int _spaces;

        public StringBuilderWithIdent(int spaces = 4)
        {
            _spaces = spaces;
        }

        public void AppendLineA(string str)
        {
            _sb.AppendLine(new string(' ', Ident * _spaces) + str);
        }
        
        public void AppendLine(string str)
        {
            Sb.AppendLine(new string(' ', Ident * _spaces) + str);
        }

        public void AppendLineNoIdent(string str)
        {
            Sb.AppendLine(str);
        }
        
        public void AppendLineNoIdentA(string str)
        {
            _sb.AppendLine(str);
        }

        public void AppendLine(object obj)
        {
            Sb.AppendLine(new string(' ', Ident * _spaces) + obj);
        }

        public void Append(string str)
        {
            Sb.Append(new string(' ', Ident * _spaces) + str);
        }

        public void Append(object obj)
        {
            Sb.Append(new string(' ', Ident * _spaces) + obj);
        }
        
        public void AppendA(object obj)
        {
            _sb.Append(new string(' ', Ident * _spaces) + obj);
        }

        public void AppendNoIdent(string str)
        {
            Sb.Append(str);
        }

        public void Replace(string s1, string s2)
        {
            _sb.Replace(s1, s2);
        }
        
        public void ReplaceB(string s1)
        {
            _sb.Replace(s1, _sbNoScoped.ToString());
        }

        public override string ToString()
        {
            return _sb.ToString();
        }
    }
}