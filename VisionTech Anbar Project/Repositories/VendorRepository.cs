using Microsoft.EntityFrameworkCore;
using VisionTech_Anbar_Project.DAL;
using VisionTech_Anbar_Project.Entities;
using VisionTech_Anbar_Project.Repositories.Base;

namespace VisionTech_Anbar_Project.Repositories;

public class VendorRepository : BaseRepository<Vendor>
{
    public VendorRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Vendor>> GetAllAsync()
    {
        return await _context.Vendors
            .Include(v => v.Packages)
            .ToListAsync();
    }

    public async Task<Vendor> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(v => v.Packages)
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task AddAsync(Vendor vendor)
    {
        await _dbSet.AddAsync(vendor);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Vendor vendor)
    {
        _dbSet.Update(vendor);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var vendor = await _dbSet.FindAsync(id);
        if (vendor != null)
        {
            _dbSet.Remove(vendor);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Vendor> FindByName(string name)
    {
        var vendor = (await GetAllAsync()).FirstOrDefault(x => x.VendorName == name);
        return vendor;
    }
}