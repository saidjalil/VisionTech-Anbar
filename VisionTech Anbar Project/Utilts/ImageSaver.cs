using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisionTech_Anbar_Project.Utilts
{
    internal class ImageSaver
    {
        public static void SaveImage(OpenFileDialog openFileDialog)
        {
            string sourceImagePath = openFileDialog.FileName;

            string projectFolder = AppDomain.CurrentDomain.BaseDirectory;

            string imagesFolder = Path.Combine(projectFolder, "Images","Documents");

            if (!Directory.Exists(imagesFolder))
            {
                Directory.CreateDirectory(imagesFolder);
            }

            // Get the file name from the source path and set the destination path
            string fileName = Path.GetFileName(sourceImagePath);
            string destinationImagePath = Path.Combine(imagesFolder, fileName);
            File.Copy(sourceImagePath, destinationImagePath, true);
        }
    }
}
