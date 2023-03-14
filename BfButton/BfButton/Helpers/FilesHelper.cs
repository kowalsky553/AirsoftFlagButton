using System;
using System.IO;

namespace BfButton.Helpers
{
    public static class FilesHelper
    {
        private static string LocalAppFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public static int ReadScore(string fileName)
        {
            return int.Parse(File.ReadAllText(string.Concat(LocalAppFolder, fileName)));
        }

        public static void WriteScore(string fileName, int value)
        {
            File.WriteAllText(string.Concat(LocalAppFolder, fileName), value.ToString());
        }

        public static void ClearAllScore()
        {
            WriteScore("Yellow.txt", 0);
            WriteScore("Green.txt", 0);
            WriteScore("Blue.txt", 0);
        }
    }
}
