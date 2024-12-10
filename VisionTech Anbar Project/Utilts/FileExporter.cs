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

    public FileExporter(PackageService packageService, ImageService imageService)
    {
        _packageService = packageService;
        _imageService = imageService;
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

        var package = await _packageService.GetPackageWithNavigation(id);
        var image = await _imageService.GetImagesByPackageIdAsync(package.Id);

        ExportViewModel export = new ExportViewModel()
        {
            Package = package,
            Image = image.First(),
        };

        var json = JsonConvert.SerializeObject(package, settings);

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

    // Parallelize the processing of packages and images
    var tasks = ids.Select(async id =>
    {
        try
        {
            var packageTask = _packageService.GetPackageWithNavigation(id);
            var imageTask = _imageService.GetImagesByPackageIdAsync(id);

            var package = await packageTask;
            var images = await imageTask;

            ExportViewModel export = new ExportViewModel
            {
                Package = package,
                Image = images.FirstOrDefault() // Get the first image if available
            };

            return export;
        }
        catch (Exception e)
        {
            Log.Information($"Error processing package with id {id}: {e.Message}");
            return null; // Return null to prevent the whole process from failing
        }
    });

    // Wait for all the tasks to complete
    var results = await Task.WhenAll(tasks);

    // Filter out any null results (if any task failed)
    exportViewModels = results.Where(x => x != null).ToList();

    // Serialize the list of export view models to JSON
    var json = JsonConvert.SerializeObject(exportViewModels, settings);

    // Write to the file asynchronously
    using (FileStream fs = File.Create(destinationFilePath))
    using (StreamWriter writer = new StreamWriter(fs))
    {
        await writer.WriteAsync(json);
    }

    // Update IsExported for each package (this can also be parallelized, but be mindful of database limits)
    var updateTasks = ids.Select(async id =>
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
    });

    await Task.WhenAll(updateTasks);
}

}