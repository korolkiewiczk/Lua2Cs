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
            
            _sb.AppendLineA(Constants.Imports);
        }

        public TypeGen Class()
        {
            return new TypeGen(_sb);
        }

        public void Save()
        {
            _sb.Replace(Constants.Imports, 
@"using System;
using System.Collections.Generic;
");
            File.WriteAllText(_fileName,_sb.ToString());
        }
    }
}