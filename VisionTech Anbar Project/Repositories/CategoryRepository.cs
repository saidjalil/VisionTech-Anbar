using Microsoft.EntityFrameworkCore;
using VisionTech_Anbar_Project.Entities.Categories;
using VisionTech_Anbar_Project.Repositories.Base;

namespace VisionTech_Anbar_Project.Repositories;

public class CategoryRepository : BaseRepository<Category>
{
    
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _dbSet
                .Include(c => c.SubCategories)
                .ToListAsync();
        }
    
        public async Task<Category> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(c => c.SubCategories)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    
        public async Task AddAsync(Category category)
        {
            await _dbSet.AddAsync(category);
            await _context.SaveChangesAsync();
        }
    
        public async Task UpdateAsync(Category category)
        {
            _dbSet.Update(category);
            await _context.SaveChangesAsync();
        }
    
        public async Task DeleteAsync(int id)
        {
            var category = await _dbSet.FindAsync(id);
            if (category != null)
            {
                _dbSet.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    
        public async Task<IEnumerable<Category>> GetSubCategoriesAsync(int parentId)
        {
            return await _dbSet
                .Where(c => c.ParentId == parentId)
                .Include(c => c.SubCategories)
                .ToListAsync();
        }
        
        public async Task<Category> FindCategoryByNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.Name == name);
        }
        
        public async Task<List<Category>> GetRootCategoriesAsync()
        {
            return await _dbSet.Where(c => c.ParentId == null).ToListAsync();
        }
}