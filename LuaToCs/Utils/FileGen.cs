using System.IO;

namespace LuaToCs.Utils
{
    public class FileGen
    {
        private readonly string _fileName;
        private readonly StringBuilderWithIdent _sb;

        public FileGen(string fileName)
        {
            _fileName = fileName;
            _sb = new StringBuilderWithIdent();
            
            _sb.Append(Constants.Imports);
        }

        public TypeGen Class()
        {
            return new TypeGen(_sb);
        }

        public void Save()
        {
            _sb.Replace(Constants.Imports, "");
            File.WriteAllText(_fileName,
                _sb.ToString()
                    //.Replace($"{Environment.NewLine};{Environment.NewLine}", Environment.NewLine)
                );
        }
    }
}