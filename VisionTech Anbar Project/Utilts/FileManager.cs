using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionTech_Anbar_Project.Utilts
{
    internal class FileManager
    {
        public static string GetAppDataPath()
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string twoFoldersUp = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(currentDirectory))));
            string filePath = Path.Combine(twoFoldersUp, "AppData");

            return filePath;
        }

        public static string GetImagesPath()
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string twoFoldersUp = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(currentDirectory))));
            string filePath = Path.Combine(twoFoldersUp, "Images","Documents");

            return filePath;
        }

        public static string GetLogPath()
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string twoFoldersUp = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(currentDirectory))));
            string filePath = Path.Combine(twoFoldersUp, "Logs");

            return filePath;
        }
    }

}
