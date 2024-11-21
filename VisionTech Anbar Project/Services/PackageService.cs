using VisionTech_Anbar_Project.Entities;
using VisionTech_Anbar_Project.Repositories;
using Serilog;

namespace VisionTech_Anbar_Project.Services;

public class PackageService
{
    private readonly PackageRepository _packageRepository;

    public PackageService(PackageRepository packageRepository)
    {
        _packageRepository = packageRepository;
    }

    public async Task<IEnumerable<Package>> GetAllPackagesAsync()
    {
        try
        {
            Log.Information("Fetching all packages with related products.");
            var packages = await _packageRepository.GetAll(x => x.PackageProducts);

            Log.Information("Successfully retrieved {Count} packages.", packages?.Count() ?? 0);
            return packages;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while fetching all packages.");
            throw;
        }
    }

    public async Task<Package> GetPackageByIdAsync(int id)
    {
        try
        {
            Log.Information("Fetching package with ID: {Id}.", id);
            var package = await _packageRepository.FindAsyncById(id, x => x.PackageProducts, x => x.Vendor, x => x.Warehouse);

            if (package == null)
            {
                Log.Warning("Package with ID: {Id} not found.", id);
            }
            else
            {
                Log.Information("Package with ID: {Id} successfully retrieved.", id);
            }

            return package;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while fetching package with ID: {Id}.", id);
            throw;
        }
    }

    public async Task CreatePackageAsync(Package package)
    {
        if (package == null)
        {
            Log.Error("Attempted to create a null package.");
            throw new ArgumentNullException(nameof(package), "Package cannot be null.");
        }

        try
        {
            Log.Information("Creating a new package with details: {Package}.", package);
            await _packageRepository.Create(package);
            Log.Information("Package successfully created.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while creating a package.");
            throw;
        }
    }

    public async Task UpdatePackageAsync(Package package)
    {
        if (package == null)
        {
            Log.Error("Attempted to update a null package.");
            throw new ArgumentNullException(nameof(package), "Package cannot be null.");
        }

        try
        {
            Log.Information("Updating package with ID: {Id}.", package.Id);
            await _packageRepository.Update(package);
            Log.Information("Package with ID: {Id} successfully updated.", package.Id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while updating package with ID: {Id}.", package.Id);
            throw;
        }
    }

    public async Task DeletePackageAsync(int id)
    {
        try
        {
            Log.Information("Deleting package with ID: {Id}.", id);
            var package = await _packageRepository.FindAsyncById(id);

            if (package == null)
            {
                Log.Warning("Package with ID: {Id} not found. Cannot delete.", id);
                return;
            }

            await _packageRepository.Remove(package);
            Log.Information("Package with ID: {Id} successfully deleted.", id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while deleting package with ID: {Id}.", id);
            throw;
        }
    }

    public async Task AddProductToPackageAsync(int packageId, int productId, int quantity)
    {
        try
        {
            Log.Information("Adding product with ID: {ProductId} to package with ID: {PackageId}, quantity: {Quantity}.", 
                productId, packageId, quantity);

            await _packageRepository.AddProductToPackageAsync(packageId, productId, quantity);
            Log.Information("Product with ID: {ProductId} successfully added to package with ID: {PackageId}.", 
                productId, packageId);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while adding product to package. Package ID: {PackageId}, Product ID: {ProductId}.", 
                packageId, productId);
            throw;
        }
    }

    public async Task<IEnumerable<Product>> GetProductsByPackageIdAsync(int packageId)
    {
        try
        {
            Log.Information("Fetching products for package with ID: {PackageId}.", packageId);
            var products = await _packageRepository.GetProductsByPackageIdAsync(packageId);

            if (products == null || !products.Any())
            {
                Log.Warning("No products found for package with ID: {PackageId}.", packageId);
            }
            else
            {
                Log.Information("Successfully retrieved {Count} products for package with ID: {PackageId}.", 
                    products.Count(), packageId);
            }

            return products;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while fetching products for package with ID: {PackageId}.", packageId);
            throw;
        }
    }
}
