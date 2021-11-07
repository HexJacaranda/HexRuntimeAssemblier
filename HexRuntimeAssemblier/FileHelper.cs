using System;
using System.IO;

namespace HexRuntimeAssemblier
{
    public static class FileHelper
    {
        public static FileStream OpenRead(string path)
        {
            string workingDirectory = Environment.CurrentDirectory;
            if (!Directory.Exists(path))
                return File.OpenRead(Path.Combine(workingDirectory, path));
            return File.OpenRead(path);
        }
    }
}
