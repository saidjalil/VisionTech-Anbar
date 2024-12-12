using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using VisionTech_Anbar_Project.Services;
using VisionTech_Anbar_Project.ViewModel;
using Package = VisionTech_Anbar_Project.Entities.Package;

namespace VisionTech_Anbar_Project.Utilts;

public class FileExporter
{
    PackageService _packageService;
    ImageService _imageService;
    IConfiguration _configuration;
    CategoryService _categoryService;

    public FileExporter(PackageService packageService, ImageService imageService, IConfiguration configuration, CategoryService categoryService)
    {
        _packageService = packageService;
        _imageService = imageService;
        _configuration = configuration;
        _categoryService = categoryService;
    }

    public async Task CreateAndWriteExportFile(int id)
    {
        var settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore, // Prevent circular references
            Formatting = Formatting.Indented // Pretty-print JSON for readability
        };

        

        var date = DateTime.Now;
        var fileNameWithSpaces = "Export-" + date + ".js";
        var fileName = fileNameWithSpaces.Replace(" ", "").Replace(":", "_");
        string destinationFilePath = Path.Combine(FileManager.GetDownloadsFolder(), fileName);

        Decoder decoder = new Decoder();
        var package = await _packageService.GetPackageWithNavigation(id);
        var images = await _imageService.GetImagesByPackageIdAsync(id);
        var hash = decoder.GenerateHash(_configuration, package);

        var export = await ExportDataMapper.MapToExportVM(_packageService,_categoryService,package, images.First(), hash);

        var json = JsonConvert.SerializeObject(export, settings);

        using (FileStream fs = File.Create(destinationFilePath))
        using (StreamWriter writer = new StreamWriter(fs))
        {
            writer.Write(json);
        }

    }

    public async Task CreateAndWriteExportFile(List<int> ids)
{
    var settings = new JsonSerializerSettings
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore, // Prevent circular references
        Formatting = Formatting.Indented // Pretty-print JSON for readability
    };

    var date = DateTime.Now;
    var fileNameWithSpaces = "Export-" + date + $"_Size-{ids.Count}" + ".json";
    var fileName = fileNameWithSpaces.Replace(" ", "").Replace(":", "_");
    string destinationFilePath = Path.Combine(FileManager.GetDownloadsFolder(), fileName);

    List<ExportViewModel> exportViewModels = new List<ExportViewModel>();

    Decoder decoder = new Decoder();

    try
    {
        foreach (var id in ids)
        {
            try
            {
                var package = await _packageService.GetPackageWithNavigation(id);
                var images = await _imageService.GetImagesByPackageIdAsync(id);
                var hash = decoder.GenerateHash(_configuration, package);
                
                var export = await ExportDataMapper.MapToExportVM(_packageService,_categoryService, package, images.First(),hash);
                
                
                exportViewModels.Add(export);
            }
            catch (Exception e)
            {
                Log.Information($"Error processing package with id {id}: {e.Message}");
            }
        }

        if (!exportViewModels.Any())
        {
            Log.Warning("No export view models created. Check if services returned any data.");
        }

        // Serialize the list of export view models to JSON
        var json = JsonConvert.SerializeObject(exportViewModels, settings);
        Log.Information($"JSON content length: {json.Length}");

        // Write to the file asynchronously
        using (FileStream fs = File.Create(destinationFilePath))
        using (StreamWriter writer = new StreamWriter(fs))
        {
            await writer.WriteAsync(json);
        }

        if (File.Exists(destinationFilePath))
        {
            Log.Information($"File successfully created at: {destinationFilePath}");
        }
        else
        {
            Log.Error("File creation failed.");
        }

        foreach (var id in ids)
        {
            try
            {
                var package = await _packageService.GetPackageWithNavigation(id);
                package.IsExported = true;
                await _packageService.UpdatePackageAsync(package);
            }
            catch (Exception e)
            {
                Log.Information($"Error updating IsExported for package with id {id}: {e.Message}");
            }
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        throw;
    }
}



}