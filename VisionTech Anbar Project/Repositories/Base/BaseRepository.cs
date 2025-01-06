using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using VisionTech_Anbar_Project.DAL;
using VisionTech_Anbar_Project.Entities.Base;

namespace VisionTech_Anbar_Project.Repositories.Base
{
    public class BaseRepository<T> where T : BaseItem
    {
        public AppDbContext _context;
        protected DbSet<T> _dbSet;


        public BaseRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }


        public async Task<EntityEntry<T>> Create(T item)
        {
            item.CreatedTime = DateTime.Now;
            var newItem =  _dbSet.Add(item);
            await Save();
            return newItem;
            
        }

        public async Task<T> FindAsyncById(int id)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<T> FindAsyncById(int id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<T> FindAsyncByIdWithNavigation(int id, Func<IQueryable<T>, IQueryable<T>> includeFunc)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();

            if (includeFunc != null)
            {
                query = includeFunc(query);
            }

            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IQueryable<T>> GetAll(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query;
        }

        public async Task<IEnumerable<T>> GetAsync()
        {
            return _dbSet.AsNoTracking().ToList();
        }

        public async Task<IEnumerable<T>> GetAsync(Func<T, bool> predicate)
        {
            return _dbSet.AsNoTracking().Where(predicate).ToList();
        }

        public async Task Remove(T item)
        {
            _context.Entry(item).State = EntityState.Detached;

            var existingEntity = await _dbSet.FirstOrDefaultAsync(p => p.Id == item.Id);
    
            if (existingEntity != null)
            {
                _dbSet.Remove(existingEntity);
                await Save(); // Make sure this is awaited
            }
        }

        public async Task Update(T item)
        {
            var existingEntity = await _dbSet.FindAsync(item.Id);
            if (existingEntity != null)
            {
                // Update the existing entity's properties
                _context.Entry(existingEntity).CurrentValues.SetValues(item);
            }
            else
            {
                // Attach the entity if it's not already tracked
                _dbSet.Attach(item);
                _context.Entry(item).State = EntityState.Modified;
            }

            await Save();
        }

        
        
        public async Task<List<T>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>> includeProperties = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includeProperties != null)
            {
                query = includeProperties(query);
            }
             
            return await query.ToListAsync();
        }
        public async Task<bool> IsExist(int id)
        {
            return await _dbSet.AnyAsync(e => e.Id == id);
        }
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
