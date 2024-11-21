using Newtonsoft.Json;
using Serilog;
using VisionTech_Anbar_Project.ViewModel;
using VisionTech_Anbar_Project.ViewModel.Categories;

namespace VisionTech_Anbar_Project.Utilts
{
    internal class JsonManager
    {
        public static List<Package> GetAllPackages()
        {
            var path = Path.Combine(FileManager.GetAppDataPath(), "db.json");

            Log.Information("Attempting to load data from JSON file at path: {FilePath}", path);

            try
            {
                string json = File.ReadAllText(path);

                var packages = JsonConvert.DeserializeObject<List<Package>>(json);

                if (packages == null)
                {
                    Log.Warning("No packages found in the JSON file at path: {FilePath}. Returning an empty list.", path);
                    return new List<Package>();
                }

                Log.Information("JSON file successfully loaded. Total records found: {TotalRecords}", packages.Count);
                return packages;
            }
            catch (FileNotFoundException ex)
            {
                Log.Error(ex, "The JSON file at path {FilePath} was not found.", path);
                throw;
            }
            catch (JsonException ex)
            {
                Log.Error(ex, "Failed to deserialize the JSON file at path {FilePath}.", path);
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An unexpected error occurred while loading packages from the JSON file at path {FilePath}.", path);
                throw;
            }
        }


        public static void AddPackage(Package package)
        {
            if (package == null)
            {
                Log.Error("Attempted to add a null package. Package cannot be null.");
                throw new ArgumentNullException(nameof(package), "Package cannot be null.");
            }

            Log.Information("Attempting to add a new package with ID {PackageId}.", package.PackageId);

            try
            {
                var packs = GetAllPackages();
                packs.Add(package);
                SaveData(packs);
                Log.Information("Package with ID {PackageId} successfully added.", package.PackageId);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to add package with ID {PackageId}.", package.PackageId);
                throw;
            }
        }


        public static Package GetPackageById(string id)
        {
            Log.Information("Attempting to retrieve package with ID {PackageId}.", id);

            var packages = GetAllPackages();
            var package = packages.FirstOrDefault(x => x.PackageId == id);

            if (package == null)
            {
                Log.Warning("Package with ID {PackageId} not found.", id);
                return null;
            }

            Log.Information("Package with ID {PackageId} successfully retrieved.", id);
            return package;
        }


        public static void EditPackageById(string id, Package package)
        {
            Log.Information("Attempting to update package with ID {PackageId}.", id);

            var packages = GetAllPackages();
            var existingPackage = packages.FirstOrDefault(x => x.PackageId == id);

            if (existingPackage == null)
            {
                Log.Warning("Package with ID {PackageId} not found. Cannot update non-existent package.", id);
                throw new KeyNotFoundException($"Package with ID {id} not found.");
            }


            existingPackage.Exported = package.Exported;
            existingPackage.Products = package.Products;
            Log.Information("Package with ID {PackageId} has been updated.", id);

            try
            {
                SaveData(packages);
                Log.Information("Changes successfully saved after updating package with ID {PackageId}.", id);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to save data after updating package with ID {PackageId}.", id);
                throw;
            }
        }


        public static void DeletePackageById(string id)
        {
            Log.Information("Attempting to delete package with ID {PackageId}.", id);

            var packages = GetAllPackages();
            var package = packages.FirstOrDefault(x => x.PackageId == id);

            if (package == null)
            {
                Log.Warning("Package with ID {PackageId} not found. Cannot delete non-existent package.", id);
                throw new KeyNotFoundException($"Package with ID {id} not found.");
            }


            packages.Remove(package);
            Log.Information("Package with ID {PackageId} removed from the list.", id);

            try
            {
                SaveData(packages);
                Log.Information("Changes successfully saved after deleting package with ID {PackageId}.", id);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to save data after deleting package with ID {PackageId}.", id);
                throw;
            }
        }


        private static void SaveData(List<Package> packages)
        {
            var path = Path.Combine(FileManager.GetAppDataPath(), "db.json");
            Log.Information("Attempting to save changes to JSON file at path: {FilePath}", path);

            try
            {
                string json = JsonConvert.SerializeObject(packages, Formatting.Indented);
                File.WriteAllText(path, json);
                Log.Information("Changes successfully saved to JSON file at {FilePath}.", path);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to save changes to JSON file at {FilePath}.", path);
                throw;
            }
        }


        public static void AddProductToPackage(Product product, string id)
        {
            if (product == null)
            {
                Log.Error("Failed to add product: Product is null.");
                throw new ArgumentNullException(nameof(product), "Product cannot be null.");
            }

            var pack = GetPackageById(id);

            if (pack == null)
            {
                Log.Error("Package with ID {PackageId} not found.", id);
                throw new InvalidOperationException($"Package with ID {id} not found.");
            }


            if (pack.Products.Contains(product))
            {
                Log.Information($"Product with ID {product.Id} is already in package ID {id}.");
                return;
            }


            pack.AddProduct(product);
            Log.Information($"Product with ID {product.Id} added to package ID {id}.");


            EditPackageById(id, pack);

            Log.Information("Changes saved for package ID {PackageId}.", id);
        }

        public static void EditProductOfPackage(Product product, string packageId)
        {
            if (product == null)
            {
                Log.Error("Failed to edit product: Product is null.");
                throw new ArgumentNullException(nameof(product), "Product cannot be null.");
            }

            var pack = GetPackageById(packageId);

            if (pack == null)
            {
                Log.Error("Package with ID {PackageId} not found.", packageId);
                throw new InvalidOperationException($"Package with ID {packageId} not found.");
            }
            var existingProduct = pack.Products.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct == null)
            {
                Log.Warning("Product with ID {ProductId} not found in package ID {PackageId}. Cannot update non-existent product.", product.Id, packageId);
                throw new InvalidOperationException($"Product with ID {product.Id} not found in package ID {packageId}.");
            }


            pack.UpdateProduct(product);
            Log.Information("Product with ID {ProductId} updated in package ID {PackageId}.", product.Id, packageId);


            EditPackageById(packageId, pack);
            Log.Information("Changes saved for package ID {PackageId}.", packageId);
        }


        public static void DeleteProductOfPackage(string packageId, string productId)
        {

            var pack = GetPackageById(packageId);

            if (pack == null)
            {
                Log.Error("Package with ID {PackageId} not found.", packageId);
                throw new InvalidOperationException($"Package with ID {packageId} not found.");
            }


            var product = pack.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null)
            {
                Log.Warning("Product with ID {ProductId} not found in package ID {PackageId}. Cannot delete non-existent product.", productId, packageId);
                throw new InvalidOperationException($"Product with ID {productId} not found in package ID {packageId}.");
            }


            pack.DeleteProduct(productId);
            Log.Information("Product with ID {ProductId} deleted from package ID {PackageId}.", productId, packageId);


            EditPackageById(packageId, pack);
            Log.Information("Changes saved for package ID {PackageId}.", packageId);
        }


        public static Category GetAllCategories()
        {
            var path = Path.Combine(FileManager.GetAppDataPath(), "categories.json");
            Log.Information("Attempting to load categories from JSON file at path: {FilePath}", path);

            try
            {
                string json = File.ReadAllText(path);
                var category = JsonConvert.DeserializeObject<Category>(json);

                if (category == null)
                {
                    Log.Warning("No categories found in JSON file at path: {FilePath}", path);
                    return null;
                }

                Log.Information("Categories successfully loaded from JSON file.");
                return category;
            }
            catch (FileNotFoundException ex)
            {
                Log.Error(ex, "JSON file not found at path: {FilePath}", path);
                throw;
            }
            catch (JsonException ex)
            {
                Log.Error(ex, "Failed to deserialize JSON file at path: {FilePath}", path);
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An unexpected error occurred while loading categories from path: {FilePath}", path);
                throw;
            }
        }


        public static void AddCategories()
        {
            //bilinmir hele
        }

        public static Category GetCategories()
        {
            var path = Path.Combine(FileManager.GetAppDataPath(), "categories.json");

            if (!File.Exists(path))
            {
                Log.Error("The JSON file at path {FilePath} does not exist.", path);
                return new Category(); // Return an empty Category object to avoid null references.
            }

            try
            {
                Log.Information("Attempting to load categories from JSON file at path: {FilePath}.", path);

                string json = File.ReadAllText(path);
                var categories = JsonConvert.DeserializeObject<Category>(json);

                if (categories == null)
                {
                    Log.Warning("No categories found in the JSON file at path {FilePath}. Returning an empty Category object.", path);
                    return new Category();
                }

                Log.Information("JSON file successfully loaded. Total subcategories found: {TotalRecords}.", categories.SubCategories?.Count ?? 0);
                return categories;
            }
            catch (JsonException jsonEx)
            {
                Log.Error(jsonEx, "Error deserializing JSON file at path {FilePath}. The file might be corrupted or in an invalid format.", path);
                return new Category(); // Return an empty Category object in case of deserialization issues.
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An unexpected error occurred while loading categories from the JSON file at path {FilePath}.", path);
                return new Category(); // Return an empty Category object for any other unexpected errors.
            }
        }


        public static List<ProductSample> GetProductSamples()
        {
            var path = Path.Combine(FileManager.GetAppDataPath(), "productSample.json");

            Log.Information("Attempting to load data from JSON file at path: {FilePath}", path);

            try
            {
                string json = File.ReadAllText(path);

                var samples = JsonConvert.DeserializeObject<List<ProductSample>>(json);

                if (samples == null)
                {
                    Log.Warning("No product samples found in the JSON file at path: {FilePath}. Returning an empty list.", path);
                    return new List<ProductSample>();
                }

                Log.Information("JSON file successfully loaded. Total records found: {TotalRecords}", samples.Count);
                return samples;
            }
            catch (FileNotFoundException ex)
            {
                Log.Error(ex, "The JSON file at path {FilePath} was not found.", path);
                throw;
            }
            catch (JsonException ex)
            {
                Log.Error(ex, "Failed to deserialize the JSON file at path {FilePath}.", path);
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An unexpected error occurred while loading packages from the JSON file at path {FilePath}.", path);
                throw;
            }
        }

        public static ProductSample GetProductSampleById(string productId)
        {
            if (string.IsNullOrWhiteSpace(productId))
            {
                Log.Error("Invalid productId provided. The productId is null, empty, or whitespace.");
                return null;
            }

            try
            {
                Log.Information("Fetching product sample with ID: {ProductId}.", productId);

                var list = GetProductSamples();
                var productSample = list.FirstOrDefault(p => p.Id == productId);

                if (productSample == null)
                {
                    Log.Warning("No product sample found with ID: {ProductId}.", productId);
                }
                else
                {
                    Log.Information("Product sample with ID: {ProductId} successfully retrieved.", productId);
                }

                return productSample;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while fetching product sample with ID: {ProductId}.", productId);
                return null;
            }
        }

    }
}