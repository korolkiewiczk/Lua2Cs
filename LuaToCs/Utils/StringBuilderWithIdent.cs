using System.Text;

namespace LuaToCs.Utils
{
    public class StringBuilderWithIdent
    {
        private StringBuilder _sb = new StringBuilder();
        public int Ident { get; set;  } = 0;
        private int _spaces;

        public StringBuilderWithIdent(int spaces = 4)
        {
            _spaces = spaces;
        }

        public void AppendLine(string str)
        {
            _sb.AppendLine(new string(' ', Ident * _spaces) + str);
        }
        
        public void AppendLineNoIdent(string str)
        {
            _sb.AppendLine(str);
        }
        
        public void AppendLine(object obj)
        {
            _sb.AppendLine(new string(' ', Ident * _spaces) + obj);
        }

        public void Append(string str)
        {
            _sb.Append(new string(' ', Ident * _spaces) + str);
        }
        
        public void Append(object obj)
        {
            _sb.Append(new string(' ', Ident * _spaces) + obj);
        }
        
        public void AppendNoIdent(string str)
        {
            _sb.Append(str);
        }

        public void Replace(string s1, string s2)
        {
            _sb.Replace(s1, s2);
        }

        public override string ToString()
        {
            return _sb.ToString();
        }
    }
}