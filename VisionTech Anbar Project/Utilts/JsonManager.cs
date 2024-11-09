using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;
using VisionTech_Anbar_Project.ViewModel;

namespace VisionTech_Anbar_Project.Utilts
{
    internal class JsonManager
    {
        public static List<Package> GetAllPackages()
        {
            var path = Path.Combine(FileManager.GetAppDataPath(), "db.json");
            

            string json = File.ReadAllText(path);

            var packages = JsonConvert.DeserializeObject<List<Package>>(json);
            return packages;
        } 

        public static void AddPackage(Package package)
        {
            var packs = GetAllPackages();
            packs.Add(package);

        }

        public static Package GetPackageById(int id) 
        {
            var packeges = GetAllPackages();
            Package package = packeges.FirstOrDefault(x => x.Id == id);

            return package;
        }

        public static void EditPackageById(int id, Package package)
        {
            var packages = GetAllPackages();
            var existingPackage = packages.FirstOrDefault(x => x.Id == id);

            if (existingPackage == null)
            {
                throw new Exception("Package not found"); 
            }

            existingPackage.Id = id;
            existingPackage.Exported = package.Exported;
            existingPackage.Products = package.Products;

            SaveData(packages);
        }

        public static void DeletePackageById(int id)
        {
            var packages = GetAllPackages();
            var package = packages.FirstOrDefault(x => x.Id == id);

            if (package != null)
            {
                packages.Remove(package);
                SaveData(packages);
            }
            else
            {
                throw new Exception("Package not found");
            }
        }

        private static void SaveData(List<Package> packages)
        {
            var path = Path.Combine(FileManager.GetAppDataPath(), "db.json");
            string json = JsonConvert.SerializeObject(packages, Formatting.Indented);
            File.WriteAllText(path, json);
        }

        public static void AddProductToPackage(Product product, int id) 
        {
            var pack = GetPackageById(id);
            pack.AddProduct(product);
            EditPackageById(id, pack);
        }

        public static void EditProductOfPackage(Product product, int packageId)
        {
            var pack = GetPackageById(packageId);
            pack.UpdateProduct(product);
            EditPackageById(packageId, pack);
        }

        public static void DeleteProductOfPackage(int packageId, int productId)
        {
            var pack = GetPackageById(packageId);
            pack.DeleteProduct(productId);
            EditPackageById(packageId, pack);
        }

    }
}
