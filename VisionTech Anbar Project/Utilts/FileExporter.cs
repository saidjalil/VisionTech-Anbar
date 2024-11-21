using Newtonsoft.Json;
using VisionTech_Anbar_Project.Entities;
using VisionTech_Anbar_Project.Repositories;
using VisionTech_Anbar_Project.Services;

namespace VisionTech_Anbar_Project.Utilts;

public class FileExporter
{
    PackageRepository _repository;
    PackageService _packageService;

    public FileExporter()
    {
        _repository = new PackageRepository();
        _packageService = new PackageService(_repository);
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

        var package = await _packageService.GetPackageByIdAsync(id);

        var json = JsonConvert.SerializeObject(package, settings);

        using (FileStream fs = File.Create(destinationFilePath))
        using (StreamWriter writer = new StreamWriter(fs))
        {
            writer.Write(json);
        }

    }

    public async Task CreateAndWriteExportFile(List<int> ids)
    {

        var date = DateTime.Now;
        var fileNameWithSpaces = "Export-" + date + $"_Size-{ids.Count}" + ".js";
        var fileName = fileNameWithSpaces.Replace(" ", "").Replace(":", "_");
        string destinationFilePath = Path.Combine(FileManager.GetDownloadsFolder(), fileName);

        List<ViewModel.Package> packages = new List<ViewModel.Package>();
        packages = JsonManager.GetAllPackages();
        //
        // foreach (var id in ids) 
        // {
        //     packages.Add(await _packageService.GetPackageByIdAsync(id));
        // }
            

        var json = JsonConvert.SerializeObject(packages);

        using (FileStream fs = File.Create(destinationFilePath))
        using (StreamWriter writer = new StreamWriter(fs))
        {
            writer.Write(json);
        }
    }
}