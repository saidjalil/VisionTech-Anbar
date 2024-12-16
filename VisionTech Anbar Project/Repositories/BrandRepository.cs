using Microsoft.EntityFrameworkCore;
using VisionTech_Anbar_Project.DAL;
using VisionTech_Anbar_Project.Entities;
using VisionTech_Anbar_Project.Repositories.Base;

namespace VisionTech_Anbar_Project.Repositories;

public class BrandRepository : BaseRepository<Brand>
{
    public BrandRepository(AppDbContext context) : base(context) { }
    
    public async Task<Brand> GetBrandWithProductsAsync(int brandId)
    {
        return await _dbSet
            .Include(b => b.Products)
            .FirstOrDefaultAsync(b => b.Id == brandId);
    }

    public async Task<IEnumerable<Brand>> GetBrandsWithProductsAsync()
    {
        return await _dbSet
            .Include(b => b.Products)
            .ToListAsync();
    }
}