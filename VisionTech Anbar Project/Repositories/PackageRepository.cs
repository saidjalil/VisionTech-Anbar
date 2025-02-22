using Microsoft.EntityFrameworkCore;
using VisionTech_Anbar_Project.DAL;
using VisionTech_Anbar_Project.Entities;
using VisionTech_Anbar_Project.Repositories.Base;

namespace VisionTech_Anbar_Project.Repositories;

public class PackageRepository : BaseRepository<Package>
{
    private ProductRepository _productRepository;

    public PackageRepository(AppDbContext context, ProductRepository productRepository) : base(context)
    {
        _productRepository = productRepository;
    }

    public async Task AddProductToPackageAsync(int packageId, int productId,string barcode, int quantity, int categoryId)
    {
        // Check if the entity already exists in the context or database
        var existingPackageProduct = await _context.PackageProducts
            .FirstOrDefaultAsync(pp => pp.PackageId == packageId && pp.ProductId == productId);


            // Create a new entity if it doesn't exist
            var packageProduct = new PackageProduct
            {
                PackageId = packageId,
                ProductId = productId,
                Quantity = quantity,
                Barcode = barcode,
                CreatedTime = DateTime.UtcNow,
                UpdatedTime = DateTime.UtcNow,
            };

            await _context.PackageProducts.AddAsync(packageProduct);
        

        // Save changes to the database
        await _context.SaveChangesAsync();
    }


    public async Task AddProductToPackageAsync(Product product, int packageId, int quantity, string barcode, int categoryId)
    {
        if (product == null)
        {
            throw new ArgumentNullException(nameof(product), "Product cannot be null.");
        }

        // Check if the product exists in the database
        var existingProduct = await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => 
                p.ProductName == product.ProductName &&
                p.CategoryId == product.CategoryId &&
                p.BrandId == product.BrandId);

        int productId;
    
        if (existingProduct == null)
        {
            // Create a new product instance instead of using the passed one
            var newProduct = new Product
            {
                ProductName = product.ProductName,
                CategoryId = categoryId,
                BrandId = product.Brand.Id,
                // Copy other necessary properties
            };
        
            // Add the new product
            var createdProduct = await _productRepository.Create(newProduct);
            productId = createdProduct.Entity.Id;
        }
        else
        {
            productId = existingProduct.Id;
        }

        // Create a new PackageProduct instance
        var packageProduct = new PackageProduct
        {
            PackageId = packageId,
            ProductId = productId,
            Quantity = quantity,
            Barcode = barcode
        };

        // Add the PackageProduct to the context
        await _context.PackageProducts.AddAsync(packageProduct);

        // Save changes to the database
        await Save();
    }



    
    public async Task<IEnumerable<Product>> GetProductsByPackageIdAsync(int packageId)
    {
        return await _context.Products
            .Where(p => p.PackageProducts.Any(pp => pp.PackageId == packageId))
            .Include(p => p.Category)         // Include the Category for each Product
            //.Include(p => p.Barcodes)         // Include the Barcodes for each Product
            .ToListAsync();
    }

    
}