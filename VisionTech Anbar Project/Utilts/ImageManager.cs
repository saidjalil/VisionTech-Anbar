using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Serilog;
using VisionTech_Anbar_Project.Services;

namespace VisionTech_Anbar_Project.Utilts
{
    public class ImageManager
    {
        private readonly ImageService _imageService;

        public ImageManager(ImageService imageService)
        {
            _imageService = imageService;
        }

        public async Task SaveImage(OpenFileDialog openFileDialog, int packageId)
        {
            if (openFileDialog == null || string.IsNullOrEmpty(openFileDialog.FileName))
            {
                Log.Error("No file selected for saving. The OpenFileDialog is null or contains an empty file path.");
                return;
            }

            string sourceImagePath = packageId.ToString();
            string imagesFolder = FileManager.GetImagesPath();

            try
            {
                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                    Log.Information("Images folder created at path: {ImagesFolderPath}", imagesFolder);
                }

                
                string fileName = Path.GetFileName(sourceImagePath);
                string destinationImagePath = Path.Combine(imagesFolder, fileName);

                
                File.Copy(sourceImagePath, destinationImagePath, true);
                Log.Information("Image saved successfully from {SourceImagePath} to {DestinationImagePath}", sourceImagePath, destinationImagePath);
                await _imageService.CreateImageAsync(new (){PackageId = packageId, Base64 = Convert.ToBase64String(File.ReadAllBytes(destinationImagePath))});
            }
            
            catch (IOException ioEx)
            {
                Log.Error(ioEx, "An I/O error occurred while saving the image from {SourceImagePath} to {ImagesFolderPath}.", sourceImagePath, imagesFolder);
            }
            catch (UnauthorizedAccessException authEx)
            {
                Log.Error(authEx, "Unauthorized access error occurred while saving the image to {ImagesFolderPath}.", imagesFolder);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An unexpected error occurred while saving the image from {SourceImagePath} to {DestinationImagePath}.", sourceImagePath, Path.Combine(imagesFolder, Path.GetFileName(sourceImagePath)));
            }
        }
        
        
    }
}
