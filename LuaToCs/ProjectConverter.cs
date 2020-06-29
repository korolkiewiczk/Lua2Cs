using System;
using System.IO;
using LuaToCs.Utils;

namespace LuaToCs
{
    public class ProjectConverter
    {
        private readonly string _startPath;
        private readonly string _outPath;

        public ProjectConverter(string startPath, string outPath)
        {
            _startPath = startPath;
            _outPath = outPath;
        }

        public void Convert()
        {
            foreach (string file in Directory.EnumerateFiles(_startPath, "*.lua", SearchOption.AllDirectories))
            {
                var relativePath = PathHelper.MakeRelativePath(_startPath, file);
                Console.WriteLine(relativePath);
                var relativeDir = Path.GetDirectoryName(relativePath);
                var outputDir = Path.Combine(_outPath, relativeDir);
                var outputFile = Path.Combine(outputDir,
                    NameResolver.ToPascalCase(Path.GetFileNameWithoutExtension(file)) + ".cs");
                Directory.CreateDirectory(outputDir);
                Console.WriteLine(outputFile);
                var fileConverter = new FileConverter(file, outputFile);
                fileConverter.Convert();
                if (!string.IsNullOrEmpty(fileConverter.CompilationErrorMessage))
                {
                    Console.WriteLine(fileConverter.CompilationErrorMessage);
                }
            }
        }
    }

    class PathHelper
    {
        /// <summary>
        /// Creates a relative path from one file or folder to another.
        /// </summary>
        /// <param name="fromPath">Contains the directory that defines the start of the relative path.</param>
        /// <param name="toPath">Contains the path that defines the endpoint of the relative path.</param>
        /// <returns>The relative path from the start directory to the end path or <c>toPath</c> if the paths are not related.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static String MakeRelativePath(String fromPath, String toPath)
        {
            if (String.IsNullOrEmpty(fromPath)) throw new ArgumentNullException("fromPath");
            if (String.IsNullOrEmpty(toPath)) throw new ArgumentNullException("toPath");

            Uri fromUri = new Uri(fromPath);
            Uri toUri = new Uri(toPath);

            if (fromUri.Scheme != toUri.Scheme)
            {
                return toPath;
            } // path can't be made relative.

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            String relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            if (toUri.Scheme.Equals("file", StringComparison.InvariantCultureIgnoreCase))
            {
                relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }

            return relativePath;
        }
    }
}