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
        var fileNameWithSpaces = "Export-" + date + $"_Size-{ids.Count}" + ".js";
        var fileName = fileNameWithSpaces.Replace(" ", "").Replace(":", "_");
        string destinationFilePath = Path.Combine(FileManager.GetDownloadsFolder(), fileName);

        List<ExportViewModel> exportViewModels = new List<ExportViewModel>();
        foreach (var id in ids) 
        {
            try
            {
                var package = await _packageService.GetPackageWithNavigation(id);
                var image = await _imageService.GetImagesByPackageIdAsync(package.Id);
                ExportViewModel export = new ExportViewModel();
                export.Package = package;
                if (image.ToList().Count > 0)
                {
                    export.Image = image.First();
                }
                
                exportViewModels.Add(export);
            }
            catch (Exception e)
            {
                Log.Information(e.Message);
                throw;
            }
            

            
        }
            
       
        var json = JsonConvert.SerializeObject(exportViewModels ,settings);

        using (FileStream fs = File.Create(destinationFilePath))
        using (StreamWriter writer = new StreamWriter(fs))
        {
            writer.Write(json);
        }

        foreach (var id in ids)
        {
            var package = await _packageService.GetPackageWithNavigation(id);
            package.IsExported = true;
            await _packageService.UpdatePackageAsync(package);
        }
    }
}