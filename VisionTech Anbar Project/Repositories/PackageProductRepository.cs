using VisionTech_Anbar_Project.DAL;
using VisionTech_Anbar_Project.Entities;
using VisionTech_Anbar_Project.Repositories.Base;

namespace VisionTech_Anbar_Project.Repositories;

public class PackageProductRepository : BaseRepository<PackageProduct>
{
    public PackageProductRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<List<PackageProduct>> GetPackageProductByProductId(int productId, int packageId)
    {
        return _dbSet.Where(x =>x.ProductId == productId && x.PackageId == packageId).ToList();
    }
}