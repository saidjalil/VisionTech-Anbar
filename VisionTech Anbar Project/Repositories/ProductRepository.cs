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

    public async Task UpdateProductBarcodes(List<PackageProduct> packageProducts, int productId, int packageId)
    {
        

        foreach (var packageProduct in packageProducts)
        {
            await _packageProductRepository.Create(new()
            {
                ProductId = productId,
                PackageId = packageId,
                Quantity = packageProduct.Quantity,
                Barcode = packageProduct.Barcode,
            });
            
            await Update(packageProduct.Product);
        }
        
        
    }

    public async void RemoveProductBarcodes(int packageId, int productId)
    {
        var packageProducts = await _packageProductRepository.GetPackageProductByProductId(packageId, productId);
        foreach (var packageProduct in packageProducts)
        {
            await _packageProductRepository.Remove(packageProduct);
        }



    }
}