using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionTech_Anbar_Project.ViewModel;

namespace VisionTech_Anbar_Project.Utilts
{
    internal class JsonManager
    {
        public static List<ViewModel.Package> GetAllPackages()
        {
            var path = Path.Combine(FileManager.GetAppDataPath(), "db.json");
            

            string json = File.ReadAllText(path);

            var packages = JsonConvert.DeserializeObject<List<ViewModel.Package>>(json);
            return packages;
        } 
    }
}
