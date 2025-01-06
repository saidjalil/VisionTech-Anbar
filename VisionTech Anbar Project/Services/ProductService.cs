using System.Collections;
using Microsoft.EntityFrameworkCore;
using VisionTech_Anbar_Project.Entities;
using VisionTech_Anbar_Project.Repositories;
using Serilog;
using VisionTech_Anbar_Project.DAL;

namespace VisionTech_Anbar_Project.Services;

public class ProductService
{
    private readonly ProductRepository _productRepository;
    private readonly BarcodeRepository _barcodeRepository;
    private readonly IDbContextFactory<AppDbContext> _contextFactory;
    private readonly BrandRepository _brandRepository;

    public ProductService(ProductRepository productRepository, BarcodeRepository barcodeRepository, IDbContextFactory<AppDbContext> contextFactory, BrandRepository brandRepository)
    {
        _productRepository = productRepository;
        _barcodeRepository = barcodeRepository;
        _contextFactory = contextFactory;
        _brandRepository = brandRepository;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        try
        {
            Log.Information("Fetching all products.");
            var products = await _productRepository.GetAsync();
            Log.Information("Successfully retrieved {Count} products.", products?.Count() ?? 0);
            return products;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while fetching all products.");
            throw;
        }
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        try
        {
            Log.Information("Fetching product with ID: {Id}.", id);
            var product = await _productRepository.FindAsyncById(id);

            if (product == null)
            {
                Log.Warning("Product with ID: {Id} not found.", id);
            }
            else
            {
                Log.Information("Product with ID: {Id} successfully retrieved.", id);
            }

            return product;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while fetching product with ID: {Id}.", id);
            throw;
        }
    }


    public async Task CreateProductAsync(Product product)
    {
        if (product == null)
        {
            Log.Error("Attempted to create a null product.");
            throw new ArgumentNullException(nameof(product), "Product cannot be null.");
        }

        try
        {
            Log.Information("Creating a new product with details: {@Product}.", product);
            await _productRepository.Create(product);
            Log.Information("Product successfully created.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while creating a product.");
            throw;
        }
    }

    public async Task UpdateProductWithBarcodes(int packageId, int productId,List<PackageProduct> packageProduct)
    {
        if (packageProduct == null)
        {
            Log.Error("Attempted to update a null product.");
            throw new ArgumentNullException(nameof(packageProduct), "Product cannot be null.");
        }

        try
        {
            // var existedProduct = await _productRepository.FindAsyncById(packageProduct.Id);
            // if (existedProduct == null)
            // {
            //     throw new ArgumentNullException(nameof(product), "That product doesn't exist.");
            // }
            
            await _productRepository.RemoveProductBarcodes(packageId, productId);
            
            
            
            //existedProduct.Barcodes = product.Barcodes;
            Log.Information("Updating product with ID: {Id}.", productId);
            await _productRepository.UpdateProductBarcodes(packageProduct, productId ,packageId);
            Log.Information("Product with ID: {Id} successfully updated.", productId);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while updating product with ID: {Id}.", productId);
            throw;
        }
    }
    public async Task UpdateProductAsync(Product product)
    {
        
        if (product == null)
        {
            Log.Error("Attempted to update a null product.");
            throw new ArgumentNullException(nameof(product), "Product cannot be null.");
        }

        try
        {
            var existedProduct = await _productRepository.FindAsyncById(product.Id);
            if (existedProduct == null)
            {
                throw new ArgumentNullException(nameof(product), "That product doesn't exist.");
            }
            //existedProduct.Barcodes.Clear();
            // foreach (var barcode in product.Barcodes)
            // {
            //     existedProduct.Barcodes.Add(barcode);
            // }
            
            
            Log.Information("Updating product with ID: {Id}.", product.Id);
            await _productRepository.Update(existedProduct);
            Log.Information("Product with ID: {Id} successfully updated.", product.Id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while updating product with ID: {Id}.", product.Id);
            throw;
        }
    }

    public async Task AddBarcodeToProductAsync(int productId,Barcode barcode)
    {
        
    }

    public async Task DeleteProductAsync(int id)
    {
        try
        {
            Log.Information("Deleting product with ID: {Id}.", id);
            var product = await _productRepository.FindAsyncById(id);

            if (product == null)
            {
                Log.Warning("Product with ID: {Id} not found. Cannot delete.", id);
                return;
            }

            await _productRepository.Remove(product);
            Log.Information("Product with ID: {Id} successfully deleted.", id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while deleting product with ID: {Id}.", id);
            throw;
        }
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
    {
        try
        {
            Log.Information("Fetching products for category ID: {CategoryId}.", categoryId);
            var products = await _productRepository.GetByCategoryIdAsync(categoryId);

            if (products == null || !products.Any())
            {
                Log.Warning("No products found for category ID: {CategoryId}.", categoryId);
            }
            else
            {
                Log.Information("Successfully retrieved {Count} products for category ID: {CategoryId}.", 
                    products.Count(), categoryId);
            }

            return products;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while fetching products for category ID: {CategoryId}.", categoryId);
            throw;
        }
    }

    public async Task<Product> GetProductByBarCode(string barcode)
    {
        var res = (await _productRepository.GetAll(x => x.PackageProducts))
            .FirstOrDefault(x => x.PackageProducts.Any(b => b.Barcode == barcode));
        
        return res;
    }

    public async Task CreateProductWithMultipleBarcode(Product product, List<string> barcodes)
    {
        if (product == null) throw new ArgumentNullException(nameof(product));
        if (barcodes == null || !barcodes.Any()) throw new ArgumentException("Barcode list cannot be null or empty.", nameof(barcodes));

        // Validate the uniqueness of barcodes
        var existingBarcodes = (await _barcodeRepository.GetAllAsync())
            .Where(b => barcodes.Contains(b.BarCode))
            .ToList();

        if (existingBarcodes.Any())
        {
            throw new InvalidOperationException($"The following barcodes already exist: {string.Join(", ", existingBarcodes.Select(b => b.BarCode))}");
        }

        foreach (var barcode in barcodes)
        {
            var bar = new Barcode()
            {
                BarCode = barcode
            };
            //product.Barcodes.Add(bar); 
        }
        
        // Add product to the context
        await _productRepository.Create(product);

        // Create Barcode entities and link to the product
        // foreach (var barcodeValue in barcodes)
        // {
        //     var barcode = new Barcode
        //     {
        //         BarCode = barcodeValue,
        //         Product = product // Set the relationship
        //     };
        //     _barcodeRepository.Create(barcode);
        // }

        
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryId(int id)
    {
        return await _productRepository.GetByCategoryIdAsync(id);
    }

    public async Task<IEnumerable<Product>> GetProductByBrandId(int brandId)
    {
        return await _productRepository.GetByBrandIdAsync(brandId);
    }

    public async Task<IEnumerable<Product>> GetAllProductsWithNavigation()
    {
        return await _productRepository.GetAllAsync(query => query
            .Include(x => x.Brand)
            .Include(x => x.Category)
            .Include(x => x.PackageProducts)
            .ThenInclude(pp => pp.Product)
            
        );
    }

    public async Task<Product> GetProductByIdWithNavigation(int id)
    {
        try
        {
            Log.Information("Fetching package with ID: {id} along with related navigation properties.", id);

            var package = await _productRepository.FindAsyncByIdWithNavigation(
                id,
                query => query
                    .Include(x => x.Brand)
                    .Include(x => x.Category)
                    .Include(x => x.PackageProducts)
                    //.ThenInclude(pp => pp.Barcodes)
                    
            );

            if (package == null)
            {
                Log.Warning("Package with ID: {id} not found.", id);
                return null; // Optionally throw an exception here if not found is considered critical.
            }

            Log.Information("Successfully retrieved package with ID: {id}.", id);
            return package;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while fetching package with ID: {id}.", id);
            throw; // Re-throwing the exception allows the caller to handle it appropriately.
        }
    }


    public async Task<List<PackageProduct>> GetPackageProductsByIds(int packageId, int productId)
    {
        return await _productRepository.GetPackageProductByIds(packageId, productId);
    }

    public async Task<List<Product>> GetProductsByBrandAndCategory(int brandId, int categoryId)
    {
        return await _productRepository.GetProductsByBrandAdnCategoryId(brandId, categoryId);
    }
}
