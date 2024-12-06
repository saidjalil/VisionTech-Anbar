using Newtonsoft.Json;
using Serilog;
using VisionTech_Anbar_Project.Utilts;


public class ImageConverter
{
    public static string ConvertImageToBase64(string imagePath)
    {
        if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
        {
            Log.Error("Invalid or non-existent image path provided.");
            return string.Empty;
        }

        try
        {
            using (Image image = Image.FromFile(imagePath))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] imageBytes = ms.ToArray();
                    string base64String = Convert.ToBase64String(imageBytes);

                    Log.Information("Image successfully converted to Base64.");
                    return base64String;
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error converting image to Base64.");
            return string.Empty;
        }
    }

    // public static void SaveImageToJson(Image image, int imageId)
    // {
    //     if (image == null)
    //     {
    //         Log.Error("Cannot save image to JSON. Provided image is null.");
    //         return;
    //     }
    //
    //     try
    //     {
    //         
    //         var base64String = ConvertImageToBase64(image);
    //
    //         if (string.IsNullOrEmpty(base64String))
    //         {
    //             Log.Error("Base64 conversion failed. Image could not be saved to JSON.");
    //             return;
    //         }
    //
    //         
    //         var img = new VisionTech_Anbar_Project.ViewModel.Image
    //         {
    //             ImgId = imageId,
    //             Base64 = base64String
    //         };
    //
    //         
    //         string jsonString = JsonConvert.SerializeObject(img, Formatting.Indented);
    //         var path = Path.Combine(FileManager.GetAppDataPath(), "imgBase64.json");
    //
    //         
    //         File.WriteAllText(path, jsonString);
    //         Log.Information("Image data successfully saved to JSON file at {FilePath}.", path);
    //     }
    //     catch (Exception ex)
    //     {
    //         Log.Error(ex, "An error occurred while saving the image to JSON at {FilePath}.", Path.Combine(FileManager.GetAppDataPath(), "imgBase64.json"));
    //     }
    // }


}
