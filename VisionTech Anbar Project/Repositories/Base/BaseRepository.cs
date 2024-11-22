using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisionTech_Anbar_Project.DAL;
using VisionTech_Anbar_Project.Entities.Base;

namespace VisionTech_Anbar_Project.Repositories.Base
{
    public class BaseRepository<T> where T : BaseItem
    {
        protected AppDbContext _context;
        protected DbSet<T> _dbSet;


        public BaseRepository()
        {
            _context = new AppDbContext();
            _dbSet = _context.Set<T>();
        }


        public async Task Create(T item)
        {
            _dbSet.Add(item);
            await Save();
        }

        public async Task<T> FindAsyncById(int id)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<T> FindAsyncById(int id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();

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
            IQueryable<T> query = _dbSet;

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
            _dbSet.Remove(item);
            await Save();
        }

        public async Task Update(T item)
        {
            _context.Entry(item).State = EntityState.Modified;
            _context.Update(item);
            await Save();
           
        }
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
