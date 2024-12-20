using Microsoft.EntityFrameworkCore;
using VisionTech_Anbar_Project.DAL;
using VisionTech_Anbar_Project.Entities;
using VisionTech_Anbar_Project.Repositories.Base;

namespace VisionTech_Anbar_Project.Repositories;

public class ProductRepository : BaseRepository<Product>
{
    PackageProductRepository _packageProductRepository;
    public ProductRepository(AppDbContext context, PackageProductRepository packageProductRepository) : base(context)
    {
        _packageProductRepository = packageProductRepository;
    }

    public async Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId)
    {
        return await _dbSet.AsNoTracking()
            .Where(p => p.CategoryId == categoryId)
            .Include(p => p.Category)
            .ToListAsync()
            .ConfigureAwait(false);;
    }

    public async Task<IEnumerable<Product>> GetByBrandIdAsync(int brandId)
    {
        return await _dbSet.AsNoTracking()
            .Where(p => p.BrandId == brandId)
            .Include(p => p.Brand)
            .ToListAsync()
            .ConfigureAwait(false);;
    }

    public async Task UpdateProductBarcodes(Product product,List<PackageProduct> packageProducts, int packageId)
    {
        var barcodes = await _packageProductRepository.GetPackageProductByProductId(product.Id,packageId);
        foreach (var barcode in barcodes)
        {
            await _packageProductRepository.Remove(barcode);
        }

        foreach (var packageProduct in packageProducts)
        {
            await _packageProductRepository.Create(new()
            {
                ProductId = product.Id,
                PackageId = packageProduct.Id,
                Quantity = packageProduct.Quantity,
                Barcode = packageProduct.Barcode,
            });
        }
        
        await Update(product);
    }
    
    
    
    
    
}