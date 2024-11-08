using System;
using System.IO;

public class ImageConverter
{
    public static string ConvertImageToBase64(string imagePath)
    {
        try
        {

            byte[] imageBytes = File.ReadAllBytes(imagePath);


            string base64String = Convert.ToBase64String(imageBytes);

            return base64String;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error converting image to Base64: " + ex.Message);
            return string.Empty;
        }
    }
}
