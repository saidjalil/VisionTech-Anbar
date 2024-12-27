using Microsoft.EntityFrameworkCore;
using VisionTech_Anbar_Project.DAL;
using VisionTech_Anbar_Project.Entities;
using VisionTech_Anbar_Project.Repositories.Base;

namespace VisionTech_Anbar_Project.Repositories;

public class ProductRepository : BaseRepository<Product>
{
    readonly PackageProductRepository _packageProductRepository;
    private readonly IDbContextFactory<AppDbContext> _contextFactory;
    public ProductRepository(AppDbContext context, PackageProductRepository packageProductRepository, IDbContextFactory<AppDbContext> contextFactory) : base(context)
    {
        _packageProductRepository = packageProductRepository;
        _contextFactory = contextFactory;
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
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Products
            .Where(p => p.BrandId == brandId)
            .ToListAsync();
    
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
    
    
    public async Task<List<PackageProduct>> GetPackageProductByIds(int packageId, int productId)
    {
        return await _packageProductRepository.GetPackageProductByProductId(packageId, productId);
    }

    public async Task<List<Product>> GetProductsByBrandAdnCategoryId(int br, int categoryId)
    {
        var products = _dbSet.AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Brand).Where(p => p.BrandId == br && p.CategoryId == categoryId);
        return products.ToList();
    }
    
}