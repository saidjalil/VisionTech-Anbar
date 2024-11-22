using Microsoft.EntityFrameworkCore;
using VisionTech_Anbar_Project.Entities;
using VisionTech_Anbar_Project.Repositories.Base;

namespace VisionTech_Anbar_Project.Repositories;

public class WarehouseRepository : BaseRepository<Warehouse>
{

    public async Task<IEnumerable<Warehouse>> GetAllAsync()
    {
        return await _dbSet
            .Include(w => w.Packages)
            .ToListAsync();
    }

    public async Task<Warehouse> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(w => w.Packages)
            .FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task AddAsync(Warehouse warehouse)
    {
        await _dbSet.AddAsync(warehouse);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Warehouse warehouse)
    {
        _dbSet.Update(warehouse);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var warehouse = await _dbSet.FindAsync(id);
        if (warehouse != null)
        {
            _dbSet.Remove(warehouse);
            await _context.SaveChangesAsync();
        }
    }
}