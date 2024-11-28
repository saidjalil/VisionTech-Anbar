using Microsoft.EntityFrameworkCore;
using VisionTech_Anbar_Project.DAL;
using VisionTech_Anbar_Project.Entities;
using VisionTech_Anbar_Project.Repositories.Base;

namespace VisionTech_Anbar_Project.Repositories;

public class PackageRepository : BaseRepository<Package>
{
    private ProductRepository _productRepository;
    public PackageRepository()
    {
        _productRepository = new ProductRepository();
    }

    public async Task AddProductToPackageAsync(int packageId, int productId, int quantity, int categoryId)
    {
        // Check if the entity already exists in the context or database
        var existingPackageProduct = await _context.PackageProducts
            .FirstOrDefaultAsync(pp => pp.PackageId == packageId && pp.ProductId == productId);

        if (existingPackageProduct != null)
        {
            // Update the existing entity
            existingPackageProduct.Quantity = quantity;

            // Update other properties if needed
            // existingPackageProduct.CategoryId = categoryId; // Uncomment if this property exists
        }
        else
        {
            // Create a new entity if it doesn't exist
            var packageProduct = new PackageProduct
            {
                PackageId = packageId,
                ProductId = productId,
                Quantity = quantity
            };

            await _context.PackageProducts.AddAsync(packageProduct);
        }

        // Save changes to the database
        await _context.SaveChangesAsync();
    }


    public async Task AddProductToPackageAsync(Product product, int packageId, int quantity, int categoryId)
    {
        if (product == null)
        {
            throw new ArgumentNullException(nameof(product), "Product cannot be null.");
        }

        // Check if the product exists in the database
        var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.ProductName == product.ProductName);
        
        if (existingProduct == null)
        {
            product.CategoryId = categoryId;    
            // Product does not exist, so add it to the database
            product = (await  _productRepository.Create(product)).Entity;
            await _context.SaveChangesAsync(); // Save to generate the Product ID
        }
        else
        {
            // Use the existing product
            product = existingProduct;
        }

        // Create a new PackageProduct instance
        var packageProduct = new PackageProduct
        {
            PackageId = packageId,
            ProductId = product.Id, // Use the newly generated or existing Product ID
            Quantity = quantity
        };

        // Add the PackageProduct to the context
        await _context.PackageProducts.AddAsync(packageProduct);

        // Save changes to the database
        await Save();
    }
    
    public async Task<IEnumerable<Product>> GetProductsByPackageIdAsync(int packageId)
    {
        return await _context.PackageProducts
            .Where(pp => pp.PackageId == packageId)
            .Select(pp => pp.Product)
            .ToListAsync();
    }

    
}