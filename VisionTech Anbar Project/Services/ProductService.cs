using System.Collections;
using VisionTech_Anbar_Project.Entities;
using VisionTech_Anbar_Project.Repositories;
using Serilog;

namespace VisionTech_Anbar_Project.Services;

public class ProductService
{
    private readonly ProductRepository _productRepository;

    public ProductService(ProductRepository productRepository)
    {
        _productRepository = productRepository;
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

    public async Task UpdateProductAsync(Product product)
    {
        if (product == null)
        {
            Log.Error("Attempted to update a null product.");
            throw new ArgumentNullException(nameof(product), "Product cannot be null.");
        }

        try
        {
            Log.Information("Updating product with ID: {Id}.", product.Id);
            await _productRepository.Update(product);
            Log.Information("Product with ID: {Id} successfully updated.", product.Id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error occurred while updating product with ID: {Id}.", product.Id);
            throw;
        }
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

    public async Task<Product> GetProductByBarCode(int barcode)
    {
        var res = (await _productRepository.GetAll()).FirstOrDefault(x => x.Barcodes.Any(x => x.BarCode == barcode));
        return res;
    }
}
