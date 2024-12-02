using Microsoft.EntityFrameworkCore;
using VisionTech_Anbar_Project.Entities;
using VisionTech_Anbar_Project.Repositories;
using Serilog;
using VisionTech_Anbar_Project.Entities.Categories;

namespace VisionTech_Anbar_Project.Services;

public class PackageService
{
    private readonly PackageRepository _packageRepository;
    private readonly VendorRepository _vendorRepository;
    private readonly WarehouseRepository _warehouseRepository;
    private readonly CategoryRepository _categoryRepository;

    

    public PackageService(PackageRepository packageRepository, VendorRepository vendorRepository, WarehouseRepository warehouseRepository, CategoryRepository categoryRepository)
    {
        _packageRepository = packageRepository;
        _vendorRepository = vendorRepository;
        _warehouseRepository = warehouseRepository;
        _categoryRepository = categoryRepository;
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

    public async Task AddProductToPackageAsync(int packageId, int productId, int quantity, Category category)
    {
        try
        {
            var categoryId = (await _categoryRepository.Create(category)).Entity.Id;
            Log.Information("Adding product with ID: {ProductId} to package with ID: {PackageId}, quantity: {Quantity}.", 
                productId, packageId, quantity);

            await _packageRepository.AddProductToPackageAsync(packageId, productId, quantity,categoryId);
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
    
    public async Task AddProductToPackageAsync(int packageId, int productId, int quantity, int categoryId)
    {
        try
        {
            Log.Information("Adding product with ID: {ProductId} to package with ID: {PackageId}, quantity: {Quantity}.", 
                productId, packageId, quantity);

            await _packageRepository.AddProductToPackageAsync(packageId, productId, quantity,categoryId);
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
    

    public async Task AddProductToPackageAsync(Product product, int packageId, int quantity, Category category)
    {
        var categoryId = (await _categoryRepository.Create(category)).Entity.Id;
        await _packageRepository.AddProductToPackageAsync(product, packageId, quantity,categoryId);
    }
    public async Task AddProductToPackageAsync(Product product, int packageId, int quantity, int categoryId)
    {
        await _packageRepository.AddProductToPackageAsync(product, packageId, quantity, categoryId);
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
    
    public async Task<Package> GetPackageWithNavigation(int packageId)
    {
        try
        {
            Log.Information("Fetching package with ID: {PackageId} along with related navigation properties.", packageId);

            var package = await _packageRepository.FindAsyncByIdWithNavigation(
                packageId,
                query => query
                    .Include(x => x.PackageProducts)
                    .ThenInclude(pp => pp.Product)
                    .ThenInclude(pp => pp.Barcodes)
                    .Include(x => x.Vendor)
                    .Include(x => x.Warehouse)
            );

            if (package == null)
            {
                Log.Warning("Package with ID: {PackageId} not found.", packageId);
                return null; // Optionally throw an exception here if not found is considered critical.
            }

            Log.Information("Successfully retrieved package with ID: {PackageId}.", packageId);
            return package;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while fetching package with ID: {PackageId}.", packageId);
            throw; // Re-throwing the exception allows the caller to handle it appropriately.
        }
    }



    public async Task<List<Package>> GetAllPackageWithNavigation()
    {
        try
        {
            Log.Information("Fetching all packages with related navigation properties.");

            // Assuming GetAll already includes navigation properties
            var packages = await _packageRepository.GetAllAsync(
                query => query
                    .Include(x => x.PackageProducts)
                    .ThenInclude(pp => pp.Product)
                    .Include(x => x.Vendor)
                    .Include(x => x.Warehouse)
            );

            Log.Information("Successfully retrieved {Count} packages.", packages.Count);
            return packages;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while fetching all packages.");
            throw; // Re-throw the exception for upstream handling.
        }
    }

    public async Task CreatePackageWithNewInputs(Package package, Vendor vendor)
    {
        if (package == null) throw new ArgumentNullException(nameof(package), "Package cannot be null.");
        if (vendor == null) throw new ArgumentNullException(nameof(vendor), "Vendor cannot be null.");

        try
        {
            Log.Information("Creating a new vendor: {Vendor}", vendor);
        
            var newVendor = await _vendorRepository.Create(vendor);
            if (newVendor.Entity == null)
            {
                Log.Warning("Failed to create vendor. Vendor data: {Vendor}", vendor);
                throw new InvalidOperationException("Vendor creation failed.");
            }

            Log.Information("Successfully created vendor with ID: {VendorId}.", newVendor.Entity.Id);

            package.VendorId = newVendor.Entity.Id;
            package.Vendor = newVendor.Entity;

            Log.Information("Creating a new package: {Package}.", package);
            await _packageRepository.Create(package);
            Log.Information("Successfully created package with ID: {PackageId}.", package.Id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while creating package with new vendor.");
            throw; // Re-throw for upstream handling.
        }
    }

    
    public async Task CreatePackageWithNewInputs(Package package, Warehouse warehouse)
    {
        if (package == null) throw new ArgumentNullException(nameof(package), "Package cannot be null.");
        if (warehouse == null) throw new ArgumentNullException(nameof(warehouse), "Warehouse cannot be null.");

        try
        {
            Log.Information("Creating a new warehouse: {Warehouse}", warehouse);

            var newWarehouse = await _warehouseRepository.Create(warehouse);
            if (newWarehouse.Entity == null)
            {
                Log.Warning("Failed to create warehouse. Warehouse data: {Warehouse}", warehouse);
                throw new InvalidOperationException("Warehouse creation failed.");
            }

            Log.Information("Successfully created warehouse with ID: {WarehouseId}.", newWarehouse.Entity.Id);

            package.WarehouseId = newWarehouse.Entity.Id;
            package.Warehouse = newWarehouse.Entity;

            Log.Information("Creating a new package: {Package}.", package);
            await _packageRepository.Create(package);
            Log.Information("Successfully created package with ID: {PackageId}.", package.Id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while creating package with new warehouse.");
            throw;
        }
    }

    public async Task CreatePackageWithNewInputs(Package package, Vendor vendor, Warehouse warehouse)
    {
        if (package == null) throw new ArgumentNullException(nameof(package), "Package cannot be null.");
        if (vendor == null) throw new ArgumentNullException(nameof(vendor), "Vendor cannot be null.");
        if (warehouse == null) throw new ArgumentNullException(nameof(warehouse), "Warehouse cannot be null.");

        try
        {
            Log.Information("Creating a new vendor: {Vendor}", vendor);

            var newVendor = await _vendorRepository.Create(vendor);
            if (newVendor.Entity == null)
            {
                Log.Warning("Failed to create vendor. Vendor data: {Vendor}", vendor);
                throw new InvalidOperationException("Vendor creation failed.");
            }

            Log.Information("Successfully created vendor with ID: {VendorId}.", newVendor.Entity.Id);

            Log.Information("Creating a new warehouse: {Warehouse}", warehouse);

            var newWarehouse = await _warehouseRepository.Create(warehouse);
            if (newWarehouse.Entity == null)
            {
                Log.Warning("Failed to create warehouse. Warehouse data: {Warehouse}", warehouse);
                throw new InvalidOperationException("Warehouse creation failed.");
            }

            Log.Information("Successfully created warehouse with ID: {WarehouseId}.", newWarehouse.Entity.Id);

            package.VendorId = newVendor.Entity.Id;
            package.Vendor = newVendor.Entity;

            package.WarehouseId = newWarehouse.Entity.Id;
            package.Warehouse = newWarehouse.Entity;

            Log.Information("Creating a new package: {Package}.", package);
            await _packageRepository.Create(package);
            Log.Information("Successfully created package with ID: {PackageId}.", package.Id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while creating package with new vendor and warehouse.");
            throw;
        }
    }

    public async Task<bool> IsExsistProductInPackage(int packageId, int productId)
    {
        var package = await _packageRepository.FindAsyncByIdWithNavigation(packageId,
            query => query
                .Include(x => x.PackageProducts));
        if (package == null)
        {
            return false;
        }

        if (package.PackageProducts.FirstOrDefault(x => x.ProductId == productId) == null)
        {
            return false;
        }
        return true;
    }
}
