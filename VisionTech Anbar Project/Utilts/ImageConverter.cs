using Serilog;
using System;
using System.IO;

public class ImageConverter
{
    public static string ConvertImageToBase64(string imagePath)
    {
        if (string.IsNullOrEmpty(imagePath))
        {
            Log.Error("The provided image path is null or empty.");
            return string.Empty;
        }

        if (!File.Exists(imagePath))
        {
            Log.Error("The image file at path {ImagePath} does not exist.", imagePath);
            return string.Empty;
        }

        try
        {
            Log.Information("Attempting to convert image at path {ImagePath} to Base64.", imagePath);

            byte[] imageBytes = File.ReadAllBytes(imagePath);
            string base64String = Convert.ToBase64String(imageBytes);

            Log.Information("Image at path {ImagePath} successfully converted to Base64.", imagePath);
            return base64String;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error converting image to Base64 for image at path {ImagePath}.", imagePath);
            return string.Empty;
        }
    }

}
