using Microsoft.EntityFrameworkCore;
using VisionTech_Anbar_Project.DAL;
using VisionTech_Anbar_Project.Entities;
using VisionTech_Anbar_Project.Repositories.Base;

namespace VisionTech_Anbar_Project.Repositories;

public class PackageRepository : BaseRepository<Package> 
{
    public PackageRepository()
    {
        
    }
    
    public async Task AddProductToPackageAsync(int packageId, int productId, int quantity)
    {
        var packageProduct = new PackageProduct
        {
            PackageId = packageId,
            ProductId = productId,
            Quantity = quantity
        };

        await _context.PackageProducts.AddAsync(packageProduct);
        await _context.SaveChangesAsync();
    }
    
    public async Task<IEnumerable<Product>> GetProductsByPackageIdAsync(int packageId)
    {
        return await _context.PackageProducts
            .Where(pp => pp.PackageId == packageId)
            .Select(pp => pp.Product)
            .ToListAsync();
    }
}