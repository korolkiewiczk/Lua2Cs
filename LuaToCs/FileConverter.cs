using System;
using System.IO;
using System.Text;

namespace LuaToCs
{
    public class FileConverter
    {
        private readonly string _inputFileName;
        private readonly string _outputFile;
        private Parser _parser;

        /// <summary>
        /// Informacja o bledzie kompilacji
        /// </summary>
        public string CompilationErrorMessage { get; set; }

        public FileConverter(string inputFileName, string outputFile)
        {
            _inputFileName = inputFileName;
            _outputFile = outputFile;
        }

        public void Convert()
        {
            var code = File.ReadAllText(_inputFileName);
            byte[] byteArray = Encoding.ASCII.GetBytes(code);
            MemoryStream stream = new MemoryStream(byteArray);
            Scanner scanner = new Scanner(stream);
            _parser = new Parser(scanner)
            {
                inputFileName = _inputFileName,
                fileName = _outputFile
            };
            try
            {
                _parser.Parse();
                _parser.env.EndClass();
                _parser.env.Save();
            }
            catch (Exception ex)
            {
                CompilationErrorMessage = ex.ToString();
            }
        }
    }
}