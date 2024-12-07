using Microsoft.EntityFrameworkCore;
using Serilog;
using VisionTech_Anbar_Project.DAL;
using VisionTech_Anbar_Project.Repositories.Base;
using Image = VisionTech_Anbar_Project.Entities.Image;

namespace VisionTech_Anbar_Project.Repositories;

public class ImageRepository : BaseRepository<Image>
{
    public ImageRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Image>> GetImagesByPackageIdAsync(int packageId)
    {
        try
        {
            return await _dbSet
                .AsNoTracking()
                .Where(image => image.PackageId == packageId)
                .ToListAsync();
        }
        catch (Exception e)
        {
            Log.Error(e.Message);
            throw;
        }
        
    }
}