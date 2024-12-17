using Microsoft.EntityFrameworkCore;
using VisionTech_Anbar_Project.DAL;
using VisionTech_Anbar_Project.Entities;
using VisionTech_Anbar_Project.Repositories.Base;

namespace VisionTech_Anbar_Project.Repositories;

public class ProductRepository : BaseRepository<Product>
{
    BarcodeRepository _barcodeRepository;
    public ProductRepository(AppDbContext context, BarcodeRepository barcodeRepository) : base(context)
    {
        _barcodeRepository = barcodeRepository;
    }

    public async Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId)
    {
        return await _context.Products
            .Where(p => p.CategoryId == categoryId)
            .Include(p => p.Category)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetByBrandIdAsync(int brandId)
    {
        return await _context.Products
            .Where(p => p.BrandId == brandId)
            .Include(p => p.Brand)
            .ToListAsync();
    }

    public async Task UpdateProductBarcodes(Product product)
    {
        var barcodes = await _barcodeRepository.GetByProductIdAsync(product.Id);
        foreach (var barcode in barcodes)
        {
            await _barcodeRepository.Remove(barcode);
        }

        await Update(product);
    }
    
    
    
}