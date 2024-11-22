using Microsoft.EntityFrameworkCore;
using VisionTech_Anbar_Project.Entities;
using VisionTech_Anbar_Project.Repositories.Base;

namespace VisionTech_Anbar_Project.Repositories;

public class BarcodeRepository : BaseRepository<Barcode>
{

    public async Task<IEnumerable<Barcode>> GetAllAsync()
    {
        return await _dbSet
            .Include(b => b.Product)
            .ToListAsync();
    }

    public async Task<Barcode> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(b => b.Product)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task AddAsync(Barcode barcode)
    {
        await _dbSet.AddAsync(barcode);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Barcode barcode)
    {
        _dbSet.Update(barcode);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var barcode = await _dbSet.FindAsync(id);
        if (barcode != null)
        {
            _dbSet.Remove(barcode);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Barcode>> GetByProductIdAsync(int productId)
    {
        return await _dbSet
            .Where(b => b.ProductId == productId)
            .Include(b => b.Product)
            .ToListAsync();
    }
}