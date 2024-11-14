using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
            string filePath = Path.Combine(twoFoldersUp, "Images", "Documents");

            return filePath;
        }

        public static string GetLogPath()
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string twoFoldersUp = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(currentDirectory))));
            string filePath = Path.Combine(twoFoldersUp, "Logs");

            return filePath;
        }

        public static string GetDownloadsFolder()
        {
            string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            return downloadsPath;
        }


        public static void CreateAndWriteExportFile(string id)
        {
            var date = DateTime.Now;
            var fileNameWithSpaces = "Export-" + date + ".js";
            var fileName = fileNameWithSpaces.Replace(" ", "").Replace(":", "_");
            string destinationFilePath = Path.Combine(GetDownloadsFolder(), fileName);

            var package = JsonManager.GetPackageById(id);

            var json = JsonConvert.SerializeObject(package);

            using (FileStream fs = File.Create(destinationFilePath))
            using (StreamWriter writer = new StreamWriter(fs))
            {
                writer.Write(json);
            }

        }
    }

}
